using _05_07b;

Console.WriteLine("Hello, Semantic Kernel Agents Collaboration!");

try
{
    var a = new AgentCollaborationPractice();

    await a.Execute(
        "gpt-3.5-turbo",
        Environment.GetEnvironmentVariable("OPENAI_APIKEY", EnvironmentVariableTarget.User));

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
