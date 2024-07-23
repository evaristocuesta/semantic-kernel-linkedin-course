// See https://aka.ms/new-console-template for more information
using _03_08b;

Console.WriteLine("Hello, Semantic Kernel Handlebars Prompts Template!");

try
{
    var result = await HandlebarsPromptTemplate.Execute(
        "gpt-3.5-turbo",
        Environment.GetEnvironmentVariable("OPENAI_APIKEY", EnvironmentVariableTarget.User));

    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}