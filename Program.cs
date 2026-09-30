using System.Diagnostics;
using TestApp.Tests;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;

namespace TestApp;

internal class Program
{
    static string SafeGet(Func<string> getter)
    {
        try
        {
            return getter() ?? "<null>";
        }
        catch
        {
            return "<not supported>";
        }
    }

    static void PrintTree(AutomationElement element, int depth = 0)
    {
        string indent = new string(' ', depth * 2);

        string name = SafeGet(() => element.Name);
        string automationId = SafeGet(() => element.AutomationId);
        string className = SafeGet(() => element.ClassName);

        Console.WriteLine(
            $"{indent}" +
            $"Type='{element.ControlType}' | " +
            $"Name='{name}' | " +
            $"AutomationId='{automationId}' | " +
            $"ClassName='{className}'");

        foreach (var child in element.FindAllChildren())
        {
            PrintTree(child, depth + 1);
        }
    }

    static void Main()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();

            Console.WriteLine("EDI AUTOMATED TEST MANAGER");
            Console.WriteLine("==========================");
            Console.WriteLine();
            Console.WriteLine("1. File - New File - Save As");
            Console.WriteLine("0. Exit");
            Console.WriteLine();

            Console.Write("Select test: ");
            string? selection = Console.ReadLine();

            Console.WriteLine();

            switch (selection)
            {
                case "1":
                    FileNewFileSaveAs.Run();
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }

            if (running)
            {
                Console.WriteLine();
                Console.WriteLine("Press ENTER to return to test manager.");
                Console.ReadLine();
            }
        }
    }
}