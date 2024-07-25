using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Experimental.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_03b;

public class AgentCraftingPractice
{
    private readonly List<IAgent> _agents = [];
    private IAgentThread? _agentThread = null;

    public async Task Execute(string model, string? apiKey)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(model);
        ArgumentNullException.ThrowIfNullOrEmpty(apiKey);

        var builder = Kernel.CreateBuilder();

        builder.Services.AddOpenAIChatCompletion(
            model,
            apiKey);

        var kernel = builder.Build();

        // create agent in code
        var codeAgent = await new AgentBuilder()
                        .WithOpenAIChatCompletion(model, apiKey)
                        .WithInstructions("Repeat the user message in the voice of a pirate " +
                        "and then end with parrot sounds.")
                        .WithName("CodeParrot")
                        .WithDescription("A fun chat bot that repeats the user message in the" +
                        " voice of a pirate.")
                        .BuildAsync();

        _agents.Add(codeAgent);

        // Create agent from file
        var pathToPlugin = Path.Combine(Directory.GetCurrentDirectory(), "Agents", "ParrotAgent.yaml");
        string agentDefinition = File.ReadAllText(pathToPlugin);

        var fileAgent = await new AgentBuilder()
            .WithOpenAIChatCompletion(model, apiKey)
            .FromTemplatePath(pathToPlugin)
            .BuildAsync();

        _agents.Add(fileAgent);

        try
        {
            foreach (var agent in _agents)
            {
                // Invoke agent plugin.
                var response =
                    await agent.AsPlugin().InvokeAsync(
                        "Practice makes perfect.",
                        new KernelArguments { { "count", 2 } }
                    );

                // Display result.
                Console.WriteLine(response ?? $"No response from agent: {agent.Id}");
            }
        }
        finally
        {
            // Clean-up (storage costs $)
            await CleanUpAsync();
            await fileAgent.DeleteAsync();
            await codeAgent.DeleteAsync();
        }

        Console.ReadLine();
    }
    private async Task CleanUpAsync()
    {
        Console.WriteLine("🧽 Cleaning up ...");

        if (_agentThread != null)
        {
            Console.WriteLine("Thread going away ...");
            _agentThread.DeleteAsync();
            _agentThread = null;
        }

        if (_agents.Any())
        {
            Console.WriteLine("Agents going away ...");
            await Task.WhenAll(_agents.Select(agent => agent.DeleteAsync()));
            _agents.Clear();
        }
    }
}
