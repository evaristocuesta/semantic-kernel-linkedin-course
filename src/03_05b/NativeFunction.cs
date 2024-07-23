using _03_05b.plugins;
using Microsoft.SemanticKernel;

namespace _03_05b;

public static class NativeFunction
{
    public static async Task<FunctionResult?> Execute(string model, string? apiKey, int numberToSquareRoot)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(model);
        ArgumentNullException.ThrowIfNullOrEmpty(apiKey);
        ArgumentOutOfRangeException.ThrowIfLessThan<int>(numberToSquareRoot, 0);

        var builder = Kernel.CreateBuilder();

        builder.Services.AddOpenAIChatCompletion(
            model,
            apiKey);

        builder.Plugins.AddFromType<MathPlugin>();

        var kernel = builder.Build();

        return await kernel.InvokeAsync(
            "MathPlugin",
            "Sqrt",
            new()
            {
                { "number", numberToSquareRoot }
            }
        );
    }
}
