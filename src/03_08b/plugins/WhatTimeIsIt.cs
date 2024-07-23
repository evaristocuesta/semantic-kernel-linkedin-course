using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace _03_08b.plugins;

public class WhatTimeIsIt
{
    [KernelFunction, Description("Get the current time")]
    public string Time(IFormatProvider? formatProvider = null) => DateTimeOffset.Now.ToString("hh:mm:ss tt", formatProvider);
}
