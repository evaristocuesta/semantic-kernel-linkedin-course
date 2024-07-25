using _05_05b;

Console.WriteLine("Hello, Semantic Kernel Agents delegation!");

try
{
    var a = new AgentDelegationPractice();

    await a.Execute(
        "gpt-3.5-turbo",
        Environment.GetEnvironmentVariable("OPENAI_APIKEY", EnvironmentVariableTarget.User));

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}