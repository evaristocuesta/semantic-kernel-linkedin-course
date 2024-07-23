using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;

namespace _03_10b;

public class HandlebarsChainingFunctions
{
    public static async Task<FunctionResult?> Execute(string model, string? apiKey, string question)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(model);
        ArgumentNullException.ThrowIfNullOrEmpty(apiKey);

        var builder = Kernel.CreateBuilder();

        builder.Services.AddOpenAIChatCompletion(
            model,
            apiKey);

        //builder.Plugins.AddFromType<WhatTimeIsIt>();

        var kernel = builder.Build();

        var pluginDirectory = Path.Combine(
            Directory.GetCurrentDirectory(),
            "plugins",
            "RoleTalk");

        kernel.ImportPluginFromPromptDirectory(pluginDirectory);

        var chainingFunctionsWithHandlebarsFunction = kernel.CreateFunctionFromPrompt(
            new()
            {
                Template = @"
                {{set ""responseAsPoliceman"" (RoleTalk-RespondAsPoliceman input) }}
                {{set ""responseAsScientific"" (RoleTalk-RespondAsScientist input) }}
                {{set ""opinionFromScientificToPoliceman"" (RoleTalk-RespondAsScientist responseAsPoliceman) }}

                {{!-- Example of concatenating text and variables to finally output it with json --}}
                {{set ""finalOutput"" (concat ""Policeman: "" responseAsPoliceman "" Scientific: "" responseAsScientific  "" Scientific to Policeman: "" opinionFromScientificToPoliceman)}}
                
                Output the following responses as is, do not modify anything:
                {{json finalOutput}}
                ",
                TemplateFormat = "handlebars"
            },
            new HandlebarsPromptTemplateFactory()
        );

        return await kernel.InvokeAsync(
            chainingFunctionsWithHandlebarsFunction,
            new() {
                    { "input", question }
            });
    }
}
