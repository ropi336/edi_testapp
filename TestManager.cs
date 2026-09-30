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