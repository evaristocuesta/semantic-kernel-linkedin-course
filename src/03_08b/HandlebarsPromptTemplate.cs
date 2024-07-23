using _03_08b.plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;

namespace _03_08b;
public static class HandlebarsPromptTemplate
{
    public static async Task<FunctionResult?> Execute(string model, string? apiKey)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(model);
        ArgumentNullException.ThrowIfNullOrEmpty(apiKey);

        var builder = Kernel.CreateBuilder();

        builder.Services.AddOpenAIChatCompletion(
            model,
            apiKey);

        builder.Plugins.AddFromType<WhatTimeIsIt>();

        var kernel = builder.Build();

        List<string> todaysCalendar = ["8am - wakeup", "9am - work", "12am - lunch", "1pm - work", "6pm - exercise", "7pm - study", "10pm - sleep"];

        var handlebarsTemplate = @"
                    Please explain in a fun way the day agenda
                    {{ set ""dayAgenda"" (todaysCalendar)}}
                    {{ set ""whatTimeIsIt"" (WhatTimeIsIt-Time) }}

                    {{#each dayAgenda}}
                        Explain what you are doing at {{this}} in a fun way.
                    {{/each}}

                    Explain what you will be doing next at {{whatTimeIsIt}} in a fun way.";

        var handlebarsFunction = kernel.CreateFunctionFromPrompt(
            new PromptTemplateConfig()
            {
                Template = handlebarsTemplate,
                TemplateFormat = "handlebars"
            },
            new HandlebarsPromptTemplateFactory()
        );

        return await kernel.InvokeAsync(
            handlebarsFunction,
            new()
            {
                { "todaysCalendar", todaysCalendar }
            });

    }
}
