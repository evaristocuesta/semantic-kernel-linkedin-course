using _03_10b;

Console.WriteLine("Hello, Semantic Kernel Handlebars Prompts Template!");

try
{
    var result = await HandlebarsChainingFunctions.Execute(
        "gpt-3.5-turbo",
        Environment.GetEnvironmentVariable("OPENAI_APIKEY", EnvironmentVariableTarget.User),
        "What's the best way to deal with a city-wide power outage?");

    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}