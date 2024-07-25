using _04_03b.plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_03b;

public static class FunctionCalling
{
    public static async Task<FunctionResult?> Execute(string model, string? apiKey, string prompt)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(model);
        ArgumentNullException.ThrowIfNullOrEmpty(apiKey);
        ArgumentNullException.ThrowIfNullOrEmpty(prompt);

        var builder = Kernel.CreateBuilder();

        builder.Services.AddOpenAIChatCompletion(
            model,
            apiKey);

        var kernel = builder.Build();

        KernelFunction kernelFunctionRespondAsScientific =
            KernelFunctionFactory.CreateFromPrompt(
                "Respond to the user question as if you were a Scientific. Respond to it as you were him, showing your personality",
                functionName: "RespondAsScientific",
                description: "Responds to a question as a Scientific.");

        KernelFunction kernelFunctionRespondAsPoliceman =
            KernelFunctionFactory.CreateFromPrompt(
                "Respond to the user question as if you were a Policeman. Respond to it as you were him, showing your personality, humor and level of intelligence.",
                functionName: "RespondAsPoliceman",
                description: "Responds to a question as a Policeman.");

        KernelPlugin roleOpinionsPlugin =
            KernelPluginFactory.CreateFromFunctions(
                "RoleTalk",
                "Responds to questions or statements asuming different roles.",
                [
                    kernelFunctionRespondAsScientific,
                    kernelFunctionRespondAsPoliceman
                ]);

        kernel.Plugins.Add(roleOpinionsPlugin);
        kernel.Plugins.AddFromType<WhatDateIsIt>();

        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };

        return await kernel.InvokePromptAsync(
            prompt,
            new(openAIPromptExecutionSettings));
    }
}
