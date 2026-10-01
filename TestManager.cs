using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using System;

namespace TestApp;
internal class TestManager : IDisposable
{
    private readonly string _ediPath;

    private Application? _app;

    private UIA3Automation? _automation;

    private int _failureCount = 0;

    public int FailureCount => _failureCount;

    public Window? MainWindow { get; private set; }

    private readonly List<string> _testResults = new();

    public void LogResult<T>(string message, T expected, T actual)
    {
        bool passed = EqualityComparer<T>.Default.Equals(expected, actual);

        string result = $"[{(passed ? "PASS" : "FAIL")}] {message}";

        _testResults.Add(result);
        Console.WriteLine(result);

        if (!passed)
        {
            _failureCount++;

            string expectedLine = $"       Expected: {expected}";
            string actualLine = $"       Actual:   {actual}";

            _testResults.Add(expectedLine);
            _testResults.Add(actualLine);

            Console.WriteLine(expectedLine);
            Console.WriteLine(actualLine);
        }
    }

    public TestManager(string ediPath)
    {
        _ediPath = ediPath;
    }

    public string SafeGet(Func<string> getter)
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
    public void PrintElement(AutomationElement element, int depth = 0)
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
            PrintElement(child, depth + 1);
        }
    }
    public void PrintTree(int depth = 0)
    {
        if (_app == null)
        {
            return;
        }

        if (MainWindow == null)
        {
            return;
        }

        PrintElement(MainWindow, 0);
    }

    //Debug function to dump all available UI elements
    public void PrintTree(AutomationElement element, int depth = 0)
    {
        PrintElement(element, 0);
    }

    public AutomationElement? FindFileNameInput(AutomationElement saveAsWindow)
    {
        var fileNameBox = saveAsWindow.FindFirstDescendant(
            cf => cf.ByControlType(ControlType.Edit)
            .And(cf.ByName("File name:")));

        if (fileNameBox == null)
        {
            Console.WriteLine("[FAIL] File name field not found.");
        }

        return fileNameBox;
    }

    public bool Launch()
    {
        _app = Application.Launch(_ediPath);
        _automation = new UIA3Automation();
        Thread.Sleep(500);
        MainWindow = _app.GetMainWindow(_automation);

        return MainWindow != null;
    }

    public AutomationElement? FindByAutomationId(string automationId)
    {
        return MainWindow?.FindFirstDescendant(
            cf => cf.ByAutomationId(automationId));
    }

    public void PrintEditableTextBoxContents(string automationId)
    {
        var editors = MainWindow?.FindFirstDescendant(
            cf => cf.ByAutomationId(automationId).And(cf.ByControlType(ControlType.Edit)));

        if (editors == null)
        {
            return;
        }

        Thread.Sleep(300);

        var textPattern = editors.Patterns.Text.Pattern;

        string text = textPattern.DocumentRange.GetText(-1);

        Console.WriteLine($"TextPattern: '{text}'");
    }

    public string GetDocumentContents()
    {
        var ediView = MainWindow?.FindFirstDescendant(
            cf => cf.ByClassName("EdiView"));

        if (ediView == null)
        {
            Console.WriteLine("EdiView not found.");
            return "null";
        }

        var editor = ediView.FindFirstChild(
            cf => cf.ByControlType(ControlType.Custom));

        if (editor == null)
        {
            Console.WriteLine("Editor not found.");
            return "null";
        }

        if (!editor.Patterns.Value.IsSupported)
        {
            Console.WriteLine("Editor does not support ValuePattern.");
            return "null";
        }


        return editor.Patterns.Value.Pattern.Value;
    }

    public void GetEdiView()
    {
        var ediView = MainWindow.FindFirstDescendant(
            cf => cf.ByClassName("EdiView"));

        if (ediView == null)
        {
            Console.WriteLine("EdiView not found.");
            return;
        }

        PrintPatterns(ediView);
    }
    public void PrintPatterns(AutomationElement element)
    {
        Console.WriteLine(
            $"Type='{element.ControlType}' | " +
            $"Name='{SafeGet(() => element.Name)}' | " +
            $"ID='{SafeGet(() => element.AutomationId)}' | " +
            $"Class='{SafeGet(() => element.ClassName)}'");

        foreach (var pattern in element.GetSupportedPatterns())
        {
            Console.WriteLine($"    Pattern: {pattern}");
        }

        foreach (var child in element.FindAllChildren())
        {
            PrintPatterns(child);
        }
    }

    public AutomationElement? FindByName(string name)
    {
        return MainWindow?.FindFirstDescendant(
            cf => cf.ByName(name));
    }

    public AutomationElement? FindTabByName(string name)
    {
        if (MainWindow == null)
        {
            return null;
        }

        var tabs = MainWindow.FindAllDescendants(
        cf => cf.ByControlType(ControlType.TabItem));

        foreach (var tab in tabs)
        {
            var title = tab.FindFirstDescendant(cf => cf.ByControlType(ControlType.Text).And(cf.ByName(name)));

            if (title != null)
            {
                return tab;
            }
        }
        return null;
    }

    public bool SelectTabByName(string name)
    {
        var tab = FindTabByName(name);

        if (tab == null)
        {
            return false;
        }

        tab.AsTabItem().Select();

        return true;
    }

    public bool TabExists(string name)
    {
        var tab = FindTabByName(name);
        if (tab != null)
        {
            return true;
        }

        else return false;
    }

    public bool CloseTabByName(string name)
    {
        var tab = FindTabByName(name);

        if (tab == null)
        {
            return false;
        }

        var closeButton = tab.FindFirstDescendant(
            cf => cf.ByControlType(ControlType.Button)
                    .And(cf.ByAutomationId("DocumentCloseButton")));

        if (closeButton == null)
        {
            return false;
        }

        closeButton.AsButton().Invoke();

        return true;
    }


    public bool InvokeButton(string automationId)
    {
        var element = FindByAutomationId(automationId);

        if (element == null)
            return false;

        element.AsButton().Invoke();
        return true;
    }
    public bool InvokeButtonByName(string name)
    {
        var element = MainWindow?.FindFirstDescendant(
            cf => cf.ByName(name));

        if (element == null)
            return false;

        element.AsButton().Invoke();
        return true;
    }
    public AutomationElement? FindWindowByName(string name)
    {
        var window = MainWindow?.FindFirstDescendant(
            cf => cf.ByControlType(ControlType.Window).And(cf.ByName(name)));
        if (window == null)
        {
            Console.WriteLine("Failed to find Window.");
            return null; 
        }

        return window;
    }
    public bool ExpandMenuItemByName(string name)
    {
        var element = MainWindow?.FindFirstDescendant(cf => cf.ByName(name));

        if (element == null)
        {
            return false;
        }

        element.Patterns.ExpandCollapse.Pattern.Expand();
        return true;
    }
    public bool InvokeMenuItemByName(string name)
    {
        var element = MainWindow?.FindFirstDescendant(cf => cf.ByName(name));

        if (element == null)
        {
            return false;
        }

        element.Patterns.Invoke.Pattern.Invoke();
        return true;
    }

    public void CloseAllDocuments()
    {
        if (MainWindow == null)
            return;

        while (true)
        {
            var closeButton = MainWindow.FindFirstDescendant(
                cf => cf.ByControlType(ControlType.Button)
                        .And(cf.ByAutomationId("DocumentCloseButton")));

            if (closeButton == null)
                break;

            closeButton.AsButton().Invoke();

            Thread.Sleep(100);
        }
    }

    public bool OpenFirstRecentDocument()
    {
        if (MainWindow == null)
            return false;

        var recentDocuments = MainWindow.FindFirstDescendant(
            cf => cf.ByControlType(ControlType.MenuItem)
                    .And(cf.ByName("Recent Documents")));

        if (recentDocuments == null)
            return false;

        var firstRecent = recentDocuments.FindFirstChild(
            cf => cf.ByControlType(ControlType.MenuItem));

        if (firstRecent == null)
            return false;

        firstRecent.Patterns.Invoke.Pattern.Invoke();

        return true;
    }

    public bool CompletePopUpProcedure(string window, string path)
    {
        var popUpWindow = FindWindowByName(window);

        if (popUpWindow == null)
        {
            return false;
        }

        var fileNameBox = FindFileNameInput(popUpWindow);

        if (fileNameBox == null)
        {
            return false;
        }

        var textBox = fileNameBox.AsTextBox();

        if (textBox == null)
        {
            return false;
        }

        textBox.Text = path;

        return true;
    }

    public bool InvokeButtonInWindow(AutomationElement window, string automationId)
    {
        var element = window?.FindFirstDescendant(
            cf => cf.ByAutomationId(automationId).And(cf.ByControlType(ControlType.Button)));

        if (element == null)
        {
            return false;
        }

        element.AsButton().Invoke();
        return true;
    }
    public bool InvokeSplitButtonInWindow(AutomationElement window, string automationId)
    {
        var element = window?.FindFirstDescendant(
            cf => cf.ByAutomationId(automationId).And(cf.ByControlType(ControlType.SplitButton)));

        if (element == null)
        {
            return false;
        }

        element.AsButton().Invoke();
        return true;
    }

    public bool CheckOverwritePrompt(AutomationElement window)
    {
        var overrideWindow = window.FindFirstDescendant(
            cf => cf.ByControlType(ControlType.Window));

        if (overrideWindow == null)
        {
            return false;
        }

        var textElements = overrideWindow?.FindAllDescendants(cf => cf.ByControlType(ControlType.Text));

        if (textElements == null)
        {
            return false;
        }

        foreach (var element in textElements)
        {
            string name = SafeGet(() => element.Name);
            if (name.Contains("already exists", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    public bool OpenFile(string path, string buttonId)
    {
        InvokeButton("PART_ActionButton");

        Thread.Sleep(300);

        if (!CompletePopUpProcedure("Open", path))
        {
            Console.WriteLine("Failed to add path to file");
            return false;
        }

        AutomationElement openWindow = FindWindowByName("Open");

        Thread.Sleep(300);

        if (!InvokeButtonInWindow(openWindow, buttonId))
        {
            Console.WriteLine("Couldn't find Buttontype, trying splitbutton");
            if (!InvokeSplitButtonInWindow(openWindow, buttonId))
            {
                Console.WriteLine("Failed to press button");
                return false;
            }
        }
        return true;
    }

    public bool CompareWithComp(string compFileName)
    {
        string compPath = TestPaths.CompFile(compFileName);

        if (!File.Exists(compPath))
        {
            Console.WriteLine();
            Console.WriteLine($"COMP file does not exist: {compFileName}");
            Console.Write("Create COMP file from current test results? (Y/N): ");

            string? input = Console.ReadLine();

            if (input?.Trim().Equals("Y", StringComparison.OrdinalIgnoreCase) == true)
            {
                Directory.CreateDirectory(
                    Path.GetDirectoryName(compPath)!);

                File.WriteAllLines(compPath, _testResults);

                Console.WriteLine($"[COMP CREATED] {compFileName}");

                return true;
            }

            Console.WriteLine("[COMP NOT CREATED]");
            return false;
        }

        string[] expected = File.ReadAllLines(compPath);
        string[] actual = _testResults.ToArray();

        bool matches = expected.SequenceEqual(actual);

        if (matches)
        {
            Console.WriteLine("===================================");
            Console.WriteLine("[COMP PASS] Results match baseline.");
            Console.WriteLine("===================================");

        }
        else
        {
            Console.WriteLine("===================================");
            Console.WriteLine("[COMP FAIL] Results differ from baseline.");
            Console.WriteLine("===================================");
        }
        Console.WriteLine($"Tasks Failed: {FailureCount}");

        return matches;
    }

    public bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    public string GetFileContents(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return "null";

        }

        return File.ReadAllText(filePath);
    }

    public bool DeleteFile(string filePath)
    {
        if (!File.Exists(filePath))
            return false;

        File.Delete(filePath);
        return true;
    }

    public void Close()
    {
        if (_app != null && !_app.HasExited)
        {
            _app.Close();
        }
    }

    public void Dispose()
    {
        Close();
        _automation?.Dispose();
        _app?.Dispose();
    }
}