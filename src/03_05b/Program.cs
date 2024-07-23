// See https://aka.ms/new-console-template for more information
using _03_05b;

Console.WriteLine("Hello, Semantic Kernel Native Functions!");

try
{
    int number = 81;

    var result = await NativeFunction.Execute(
        "gpt-3.5-turbo",
        Environment.GetEnvironmentVariable("OPENAI_APIKEY", EnvironmentVariableTarget.User),
        number);

    Console.WriteLine($"The square root of {number} is {result}");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}