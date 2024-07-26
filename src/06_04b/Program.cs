using _06_04b;

Console.WriteLine("Hello, Semantic Kernel Memories!");

try
{
    var a = new ImplementingMemoriesPractice();

    await a.Execute(
        "gpt-3.5-turbo",
        "text-embedding-ada-002",
        Environment.GetEnvironmentVariable("OPENAI_APIKEY", EnvironmentVariableTarget.User));

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
