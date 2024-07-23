using Microsoft.SemanticKernel;
using System.Net.Http.Headers;

namespace _03_03b;

public static class PrompFunctions
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

        //return await CreateAndExecutePrompt(kernel, prompt);

        return await ImportPluginFromFolderAndExecuteIt(kernel, prompt);
    }

    private static async Task<FunctionResult?> CreateAndExecutePrompt(Kernel kernel, string prompt)
    {
        var kernelFunction = kernel.CreateFunctionFromPrompt(prompt);
        return await kernelFunction.InvokeAsync(kernel);
    }

    private static async Task<FunctionResult?> ImportPluginFromFolderAndExecuteIt(Kernel kernel, string prompt)
    {
        // Import a plugin from a prompt directory
        var summarizePluginDirectory = Path.Combine(
            System.IO.Directory.GetCurrentDirectory(),
            "plugins",
            "SummarizePlugin");

        kernel.ImportPluginFromPromptDirectory(summarizePluginDirectory);

        return await kernel.InvokeAsync(

              "SummarizePlugin",
              "Summarize",
              new() {
                { "input", prompt }
              });
    }
}
