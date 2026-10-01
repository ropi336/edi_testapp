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
internal class SwitchTabs
{
    public static void Run()
    {
        string ediPath = TestPaths.EdiPath;

        string filePathCopyPaste = TestPaths.TestFile("CopyPaste.txt");
        string filePathCut = TestPaths.TestFile("Cut.txt");
        string filePathOpenRecent = TestPaths.TestFile("OpenRecent.txt");

        const string cPopUpOpenButtonId = "1";

        using var edi = new TestManager(ediPath);

        edi.Launch();

        Console.WriteLine($"Preparing test case prerequisites.");

        Console.WriteLine($"Closing all documents.");

        edi.CloseAllDocuments();

        Console.WriteLine($"Opening files CopyPaste.txt, Cut.txt, and OpenRecent.txt.");

        edi.OpenFile(filePathCopyPaste, cPopUpOpenButtonId);
        edi.OpenFile(filePathCut, cPopUpOpenButtonId);
        edi.OpenFile(filePathOpenRecent, cPopUpOpenButtonId);

        Console.WriteLine($"Closing EDI.");

        Thread.Sleep(1000);
        edi.Close();
        Thread.Sleep(1000);

        Console.WriteLine($"Commencing test.");

        edi.LogResult("Launching EDI", true, edi.Launch());

        Thread.Sleep(1000);

        edi.LogResult("Search for any tab named CopyPaste.txt", true, edi.SelectTabByName("CopyPaste.txt"));

        Thread.Sleep(1000);

        edi.LogResult("Search for any tab named Cut.txt", true, edi.SelectTabByName("Cut.txt"));

        Thread.Sleep(1000);

        edi.LogResult("Search for any tab named OpenRecent.txt", true, edi.SelectTabByName("OpenRecent.txt"));

        Thread.Sleep(1000);

        edi.CloseAllDocuments();

        edi.Close();

        edi.CompareWithComp("SwitchTabs.comp");

        Console.WriteLine("App successfully shut down.");
    }
}