using AdamsCodeChallange.ConsoleApp;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

public class Program
{

    public static async Task Main(string[] args)
    {
        await Commands.PromptAndCompare();

        Console.WriteLine();
        Console.WriteLine("----");

        await Commands.RepeatProgram();
    }
}