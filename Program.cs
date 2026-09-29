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

        var newButton = window.FindFirstDescendant(
        cf => cf.ByAutomationId("New"));

        Console.WriteLine($"Found 'New' button. Invoking button.");

        //Use INVOKE, not click. Invocation always more robust than click
        newButton.AsButton().Invoke();

        //Sleep arbitrary amount to make sure action is completed
        //TODO: Wait for UI element rather than arbitrary time
        Thread.Sleep(500);

        var tabs = window.FindAllDescendants(cf => cf.ByControlType(ControlType.TabItem));

        AutomationElement? untitledTab = null;

        foreach (var tab in tabs)
        {
            var title = tab.FindFirstDescendant(
                cf => cf.ByName("Untitled.txt")
                        .And(cf.ByControlType(ControlType.Text)));

            if (title != null)
            {
                untitledTab = tab;
                break;
            }
        }

        if (untitledTab == null)
        {
            Console.WriteLine("FAIL: Untitled.txt tab was not found.");
            return;
        }

        Console.WriteLine("Found the Untitled.txt document tab.");

        //Use SELECT, not click. Select always more robust than a click.
        untitledTab.AsTabItem().Select();

        //PrintTree(window);

        //Auto close the application after 3 seconds.
        Console.WriteLine("Press ENTER to close Edi.");
        Console.ReadLine();

        application.Close();
    }
}