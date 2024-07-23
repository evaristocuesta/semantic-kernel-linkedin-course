using Microsoft.SemanticKernel;

namespace _02_05b;

public static class TryingOutTheKernel
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

        return await kernel.InvokePromptAsync(prompt);
    }
}
