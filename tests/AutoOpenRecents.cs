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
internal class AutoOpenRecents
{
    public static void Run()
    {
        string ediPath = @"D:\CPROJECT\Edi-1.2\Debug\Edi.exe";

        string filePathCopyPaste = TestPaths.TestFile("CopyPaste.txt");
        string filePathCut = TestPaths.TestFile("Cut.txt");
        string filePathOpenRecent = TestPaths.TestFile("OpenRecent.txt");

        const string cPopUpOpenButtonId = "1";

        using var edi = new TestManager(ediPath);

        edi.LogResult("Launching EDI", true, edi.Launch());

        Console.WriteLine($"Preparing test case prerequisites.");

        edi.CloseAllDocuments();
        edi.OpenFile(filePathCopyPaste, cPopUpOpenButtonId);
        edi.OpenFile(filePathCut, cPopUpOpenButtonId);
        edi.OpenFile(filePathOpenRecent, cPopUpOpenButtonId);
        Thread.Sleep(1000);
        edi.Close();
        Thread.Sleep(1000);
        edi.Launch();

        Console.WriteLine($"Commencing test.");

        edi.LogResult("Search for any tab named CopyPaste.txt", true, edi.TabExists("CopyPaste.txt"));
        edi.LogResult("Search for any tab named Cut.txt", true, edi.TabExists("Cut.txt"));
        edi.LogResult("Search for any tab named OpenRecent.txt", true, edi.TabExists("OpenRecent.txt"));

        Thread.Sleep(1000);

        edi.CloseAllDocuments();

        edi.Close();

        Console.WriteLine("App successfully shut down.");
    }
}