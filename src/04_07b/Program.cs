using _04_07b;
using Microsoft.SemanticKernel.ChatCompletion;

Console.WriteLine("Hello, Semantic Kernel Stepwise planners!");

string planPrompt = "This is the user question to my expert friends:" +
        "---" +
        "User Question: " +
        "I am being attacked by a thug which wants to rob me, what do the superheroes recommend me to do in my position? I am weak, no combat skills and not a good runner... " +
        "---" +
        "Please take this question as input for getting the expert opinions, Mr. Policeman, Scientist suggestions. Do not modify the input." +
        "Use the plugin roleTalk to get the suggestions and opinions of the experts." +
        "In addition state each expert opinion on each other stated opinions." +
        "Put the expert responses preceded with EXPERT SUGGESTIONS: and inside that preceed with Policeman: and Scientist: for clarity." +
        "Perform this with the following steps: " +
        "1. Get the suggestions from each the experts." +
        "2. Get the opinions of each expert on the other expert suggestions." +
        "3. Return the results in the format: " +
        "Expert SUGGESTIONS: Policeman: <suggestion> Scientist: <suggestion> " +
        "OPINIONS: Policeman: <opinion on Scientist> Scientist: <opinion on Policeman> " +
        "IMPORTANT: on the plan ensure that the user question is asigned to a variable and used as input. Do not modify the user question input.";

try
{
    var result = await StepwisePlannerPractice.Execute(
        "gpt-3.5-turbo",
        Environment.GetEnvironmentVariable("OPENAI_APIKEY", EnvironmentVariableTarget.User),
        planPrompt);

    Console.WriteLine($"Result: {result.FinalAnswer}");
    Console.WriteLine($"Iterations needed: {result.Iterations}");

    //print the Contents of ChatHistory
    ChatHistory chatMessageContents = result.ChatHistory;

    foreach (var chatMessage in chatMessageContents)
    {
        Console.WriteLine($"ChatMessage: {chatMessage}");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
