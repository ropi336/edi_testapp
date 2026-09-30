using System.Windows.Forms;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestApp.Tests;
internal class FileOpen
{
    public static void Run()
    {
        string ediPath = @"D:\CPROJECT\Edi-1.2\Debug\Edi.exe";

        using var edi = new TestManager(ediPath);

        string filePath = TestPaths.TestFile("TextContent.txt");

        const string cPopUpOpenButtonId = "1";

        const string cExpectedFileContents = "SampleText";

        edi.LogResult("Launching EDI", true, edi.Launch());

        edi.LogResult("Expanding 'File' menu item", true, edi.ExpandMenuItemByName("File"));

        edi.LogResult("Expanding 'Open' menu item", true, edi.ExpandMenuItemByName("Open"));

        edi.LogResult("Invoke 'Text files' Menu item", true, edi.InvokeMenuItemByName("Text files"));

        Thread.Sleep(300);

        edi.CompletePopUpProcedure("Open", filePath);

        AutomationElement openWindow = edi.FindWindowByName("Open");

        edi.LogResult("Invoking 'Open' Button.", true, edi.InvokeButtonInWindow(openWindow, cPopUpOpenButtonId));

        Thread.Sleep(200);

        edi.LogResult("Compare document contents after opening document.", cExpectedFileContents, edi.GetDocumentContents());

        Thread.Sleep(1000);

        edi.CloseAllDocuments();

        edi.Close();

        Console.WriteLine("App successfully shut down.");
    }
}