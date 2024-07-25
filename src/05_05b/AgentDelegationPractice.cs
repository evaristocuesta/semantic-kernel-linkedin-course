using _05_05b.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Experimental.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_05b;

public class AgentDelegationPractice
{
    private readonly List<IAgent> _agents = [];
    IAgentThread? _agentsThread = null;

    public async Task Execute(string model, string? apiKey)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(model);
        ArgumentNullException.ThrowIfNullOrEmpty(apiKey);

        var builder = Kernel.CreateBuilder();

        builder.Services.AddOpenAIChatCompletion(
            model,
            apiKey);

        var kernel = builder.Build();

        // Preparation for agents creation
        var menuPlugin = KernelPluginFactory.CreateFromType<MenuPlugin>();
        var pathToParrotAgent = Path.Combine(Directory.GetCurrentDirectory(), "Agents", "ParrotAgent.yaml");
        var pathToToolAgent = Path.Combine(Directory.GetCurrentDirectory(), "Agents", "ToolAgent.yaml");

        // Create agents
        var menuAgent =
            Track(
                await new AgentBuilder()
                    .WithOpenAIChatCompletion(model, apiKey)
                    .FromTemplatePath(pathToToolAgent)
                    .WithDescription("Answer questions about the menu by using the menuPlugin tool.")
                    .WithPlugin(menuPlugin)
                    .BuildAsync());

        var parrotAgent =
            Track(
                await new AgentBuilder()
                    .WithOpenAIChatCompletion(model, apiKey)
                    .FromTemplatePath(pathToParrotAgent)
                    .BuildAsync());

        var toolAgent =
            Track(
                await new AgentBuilder()
                    .WithOpenAIChatCompletion(model, apiKey)
                    .FromTemplatePath(pathToToolAgent)
                    .WithPlugin(parrotAgent.AsPlugin())
                    .WithPlugin(menuAgent.AsPlugin())
                    .BuildAsync());

        var messages = new string[]
        {
            "What is on today's menu? ",
            "how much does the Eye Steak with veggies cost? ",
            "Can you talk like pirate?",
            "Thank you",
        };

        // note that threads aren't attached to specific agents
        _agentsThread = await toolAgent.NewThreadAsync();

        try
        {
            foreach (var message in messages)
            {
                var responseMessages =
                  await _agentsThread.InvokeAsync(toolAgent, message).ToArrayAsync();

                DisplayMessages(responseMessages, toolAgent);
            }
        }
        finally
        {
            await CleanUpAsync();
        }
    }

    private IAgent Track(IAgent agent)
    {
        _agents.Add(agent);

        return agent;
    }

    private void DisplayMessages(IEnumerable<IChatMessage> messages, IAgent? agent = null)
    {
        foreach (var message in messages)
        {
            DisplayMessage(message, agent);
        }
    }

    private void DisplayMessage(IChatMessage message, IAgent? agent = null)
    {
        Console.WriteLine($"[{message.Id}]");
        if (agent != null)
        {
            Console.WriteLine($"# {message.Role}: ({agent.Name}) {message.Content}");
        }
        else
        {
            Console.WriteLine($"# {message.Role}: {message.Content}");
        }
    }
    private async Task CleanUpAsync()
    {
        if (_agentsThread != null)
        {
            _agentsThread.DeleteAsync();
            _agentsThread = null;
        }

        if (_agents.Any())
        {
            await Task.WhenAll(_agents.Select(agent => agent.DeleteAsync()));
            _agents.Clear();
        }
    }
}
