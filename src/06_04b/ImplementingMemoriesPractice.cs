using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Plugins.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_04b;
internal class ImplementingMemoriesPractice
{
    private const string _memoryCollectionName = "aboutMe";

    public async Task Execute(string model, string embeddingModelDeploymentName, string? apiKey)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(model);
        ArgumentNullException.ThrowIfNullOrEmpty(apiKey);

        var builder = Kernel.CreateBuilder();

        builder.Services.AddOpenAIChatCompletion(
            model,
            apiKey);

        var kernel = builder.Build();

        IMemoryStore memoryStore = new VolatileMemoryStore();

        ISemanticTextMemory textMemory = new MemoryBuilder()
            .WithOpenAITextEmbeddingGeneration(
                embeddingModelDeploymentName,
                apiKey)
            .WithMemoryStore(memoryStore)
            .Build();

        // Creating and Adding the memory plugin to the kernel
        var memoryPlugin = kernel.ImportPluginFromObject(new TextMemoryPlugin(textMemory));

        // Adding some memories
        await kernel.InvokeAsync(memoryPlugin["Save"], new()
        {
            [TextMemoryPlugin.InputParam] = "I live in Zurich. ",
            [TextMemoryPlugin.CollectionParam] = _memoryCollectionName,
            [TextMemoryPlugin.KeyParam] = "info5",
        });

        await kernel.InvokeAsync(memoryPlugin["Save"], new()
        {
            [TextMemoryPlugin.InputParam] = "I love learning, AI, XR and complex challenges. ",
            [TextMemoryPlugin.CollectionParam] = _memoryCollectionName,
            [TextMemoryPlugin.KeyParam] = "info6",
        });

        // Recalling memories
        // string ask = "Where do I live?";
        string ask = "What do I love?";
        Console.WriteLine($"Ask: {ask}");

        var result = await kernel.InvokeAsync(memoryPlugin["Recall"], new()
        {
            [TextMemoryPlugin.InputParam] = ask,
            [TextMemoryPlugin.CollectionParam] = _memoryCollectionName,
            [TextMemoryPlugin.LimitParam] = "2",
            [TextMemoryPlugin.RelevanceParam] = "0.79",
        });

        Console.WriteLine($"Answer: {result.GetValue<string>()}");
    }
}
