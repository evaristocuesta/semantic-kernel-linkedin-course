using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_07b;

public static class StepwisePlannerPractice
{
    public static async Task<FunctionCallingStepwisePlannerResult> Execute(string model, string? apiKey, string prompt)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(model);
        ArgumentNullException.ThrowIfNullOrEmpty(apiKey);
        ArgumentNullException.ThrowIfNullOrEmpty(prompt);

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
                new[] {
                    kernelFunctionRespondAsScientific,
                    kernelFunctionRespondAsPoliceman
                      });
        
        kernel.Plugins.Add(roleOpinionsPlugin);

        var planner = new FunctionCallingStepwisePlanner();
        
        return await planner.ExecuteAsync(kernel, prompt);
    }
}
