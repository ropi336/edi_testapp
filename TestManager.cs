using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;

namespace TestApp;
internal class TestManager : IDisposable
{
    private readonly string _ediPath;

    private Application? _app;
    private UIA3Automation? _automation;

    public Window? MainWindow { get; private set; }

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

    public AutomationElement? FindByName(string name)
    {
        return MainWindow?.FindFirstDescendant(
            cf => cf.ByName(name));
    }

    public bool InvokeButton(string automationId)
    {
        var element = FindByAutomationId(automationId);

        if (element == null)
            return false;

        element.AsButton().Invoke();
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