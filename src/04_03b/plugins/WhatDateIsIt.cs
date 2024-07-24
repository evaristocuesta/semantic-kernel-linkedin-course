using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_03b.plugins;

public class WhatDateIsIt
{
    [KernelFunction, Description("Get the current date")]
    public string Date(IFormatProvider? formatProvider = null) => DateTimeOffset.UtcNow.ToString("D", formatProvider);
}
