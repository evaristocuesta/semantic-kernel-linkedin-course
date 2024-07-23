using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace _03_05b.plugins;

public class MathPlugin
{
    [KernelFunction, Description("Take the square root of a number")]
    public double Sqrt(
        [Description("The number to take the square root of")]
        double number)
    {
        return Math.Sqrt(number); 
    }
}
