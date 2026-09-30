using System.Text.RegularExpressions;
using System.Windows.Forms;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using Microsoft.VisualBasic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestApp.Tests;
internal class CloseDocument
{
    public static void Run()
    {
        string ediPath = @"D:\CPROJECT\Edi-1.2\Debug\Edi.exe";

        string filePath = TestPaths.TestFile("CopyPaste.txt");

        const string cPopUpOpenButtonId = "1";

        using var edi = new TestManager(ediPath);

        edi.LogResult("Launching EDI", true, edi.Launch());

        Console.WriteLine($"Preparing test case prerequisites.");

        edi.CloseAllDocuments();

        Console.WriteLine($"Commencing test.");

        edi.LogResult("Open file CopyPaste.txt", true, edi.OpenFile(filePath, cPopUpOpenButtonId));
        Thread.Sleep(1000);

        edi.LogResult("Search for any tab named CopyPaste.txt", true, edi.TabExists("CopyPaste.txt"));
        Thread.Sleep(200);

        edi.LogResult("Close any tabs named CopyPaste.txt", true, edi.CloseTabByName("CopyPaste.txt"));

        Thread.Sleep(200);

        edi.LogResult("Verify tabs named CopyPaste.txt have been closed", false, edi.TabExists("CopyPaste.txt"));

        Thread.Sleep(1000);

        edi.CloseAllDocuments();

        edi.Close();

        Console.WriteLine("App successfully shut down.");
    }
}