using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning.Handlebars;

namespace _04_05b;

public static class HandlebarsPlannerPractice
{
    public static async Task<string> Execute(string model, string? apiKey, string prompt)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(model);
        ArgumentNullException.ThrowIfNullOrEmpty(apiKey);

        var builder = Kernel.CreateBuilder();

        builder.Services.AddOpenAIChatCompletion(
            model,
            apiKey);

        var kernel = builder.Build();

        var kernelFunctionRespondAsScientific = kernel.CreateFunctionFromPrompt(
            new PromptTemplateConfig()
            {
                Name = "RespondAsScientific",
                Description = "Respond as if you were a Scientific.",
                Template = @"After the user request/question, 
                    {{$input}},
                    Respond to the user question as if you were a Scientific. 
                    Respond to it as you were him, showing your personality",
                TemplateFormat = "semantic-kernel",
                InputVariables = [
                    new() { Name = "input" }
                ]
            });

        var kernelFunctionRespondAsPoliceman = kernel.CreateFunctionFromPrompt(
            new PromptTemplateConfig()
            {
                Name = "RespondAsPoliceman",
                Description = "Respond as if you were a Policeman.",
                Template = @"After the user request/question, 
                    {{$input}},
                    Respond to the user question as if you were a Policeman, showing your personality, 
                    humor and level of intelligence.",
                TemplateFormat = "semantic-kernel",
                InputVariables = [
                    new() { Name = "input" }
                ]
            });

        KernelPlugin roleOpinionsPlugin =
            KernelPluginFactory.CreateFromFunctions(
                "roleTalk",
                "Responds to questions or statements asuming different roles.",
                [
                    kernelFunctionRespondAsScientific,
                    kernelFunctionRespondAsPoliceman
                ]);

        kernel.Plugins.Add(roleOpinionsPlugin);

        var planner = new HandlebarsPlanner(new HandlebarsPlannerOptions() { AllowLoops = false });
        var plan = await planner.CreatePlanAsync(kernel, prompt);
        return await plan.InvokeAsync(kernel);
    }
}
