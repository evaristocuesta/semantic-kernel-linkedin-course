// See https://aka.ms/new-console-template for more information
using _04_03b;

Console.WriteLine("Hello, Semantic Kernel Function Calling!");

string userPrompt = "I just woke up and found myself in the middle of nowhere, " +
            "do you know what date is it? and what would a policeman and a scientist do in my place?" +
            "Please provide me the date using the WhatDateIsIt plugin and the Date function, and then " +
            "the responses from the policeman and the scientist, on this order. " +
            "For this two responses, use the RoleTalk plugin and the RespondAsPoliceman and RespondAsScientific functions.";

try
{
    var result = await FunctionCalling.Execute(
        "gpt-3.5-turbo",
        Environment.GetEnvironmentVariable("OPENAI_APIKEY", EnvironmentVariableTarget.User),
        userPrompt);

    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
