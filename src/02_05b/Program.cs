// See https://aka.ms/new-console-template for more information
using _02_05b;

Console.WriteLine("Hello, Semantic Kernel!");

try
{
    var topic = "The Semantic Kernel SDK has been born and is out to the world on December 19th, now all .NET developers are AI developers...";
    var prompt = $"Generate a very short funny poem about the given event. Be creative and be funny. Make the words ryhme together. Let your imagination run wild. Event: {topic}";

    var result = await TryingOutTheKernel.Execute(
        "gpt-3.5-turbo",
        Environment.GetEnvironmentVariable("OPENAI_APIKEY", EnvironmentVariableTarget.User), 
        prompt);

    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}