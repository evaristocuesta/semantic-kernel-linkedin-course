using _05_03b;

Console.WriteLine("Hello, Semantic Kernel Agents!");

try
{
    var a = new AgentCraftingPractice();

    await a.Execute(
        "gpt-3.5-turbo",
        Environment.GetEnvironmentVariable("OPENAI_APIKEY", EnvironmentVariableTarget.User));

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

