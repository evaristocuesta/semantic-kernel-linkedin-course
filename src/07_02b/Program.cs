using _07_02b;

Console.WriteLine("Hello, Semantic Kernel Final Research Project!");

try
{
    var a = new Researcher();

    await a.ExecuteAsync(
        "gpt-3.5-turbo",
        Environment.GetEnvironmentVariable("OPENAI_APIKEY", EnvironmentVariableTarget.User),
        Environment.GetEnvironmentVariable("BING_APIKEY", EnvironmentVariableTarget.User));

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
