using System.Diagnostics;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;

namespace Edi.TestApp;

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
    static void Main()
    {
        string ediPath = @"D:\CPROJECT\Edi-1.2\Debug\Edi.exe";

        Console.WriteLine("Starting Edi");

        using var application = Application.Launch(ediPath);

        using var automation = new UIA3Automation();

        var window = application.GetMainWindow(automation);

        if (window == null)
        {
            Console.WriteLine("FAIL: Could find Edi window.");
            return;
        }

        Console.WriteLine($"Found window: {window.Title}");

        // Look at every button FlaUI can see
        var buttons = window.FindAllDescendants(
            cf => cf.ByControlType(ControlType.Button));

        Console.WriteLine($"Found {buttons.Length} buttons:");

        foreach (var button in buttons)
        {
            Console.WriteLine(
                $"Name='{button.Name}', " +
                $"AutomationId='{button.AutomationId}'");
        }

        var newButton = window.FindFirstDescendant(
        cf => cf.ByAutomationId("New"));

        //Use INVOKE, not click. Invocation always more robust than click
        newButton.AsButton().Invoke();

        //Sleep arbitrary amount to make sure action is completed
        Thread.Sleep(500);

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

        PrintTree(window);

        Console.WriteLine();
        Console.WriteLine("Press ENTER to close Edi.");
        Console.ReadLine();

        application.Close();
    }
}