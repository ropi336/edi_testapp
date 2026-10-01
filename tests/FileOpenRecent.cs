using System.Windows.Forms;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

//======================
// Undo & Redo
//=======================
// Purpose: Verify that the most recently opened file
// is bumped to the top in the list, and correctly opens the expected file
//=======================

namespace TestApp.Tests;
internal class FileOpenRecent
{
    public static void Run()
    {
        using var edi = new TestManager(TestPaths.EdiPath);

        string filePath = TestPaths.TestFile("OpenRecent.txt");

        const string cExpectedFileContents = "OpenRecent";

        const string cPopUpOpenButtonId = "1";

        edi.LogResult("Launching EDI", true, edi.Launch());

        Console.WriteLine($"Preparing test case prerequisites.");

        edi.CloseAllDocuments();

        edi.OpenFile(filePath, cPopUpOpenButtonId);

        Thread.Sleep(200);

        edi.CloseAllDocuments();

        Thread.Sleep(200);

        Console.WriteLine($"Commencing test.");

        edi.LogResult("Expand 'File' menu item", true, edi.ExpandMenuItemByName("File"));

        edi.LogResult("Expand 'Open' menu item", true, edi.ExpandMenuItemByName("Open"));

        edi.LogResult("Expand recent documents menu item", true, edi.ExpandMenuItemByName("Recent Documents"));

        edi.LogResult("Open most recent document.", true, edi.OpenFirstRecentDocument());

        Thread.Sleep(200);

        edi.LogResult("Compare document contents after opening document.", cExpectedFileContents, edi.GetDocumentContents());

        Thread.Sleep(1000);

        edi.CloseAllDocuments();

        edi.Close();

        edi.CompareWithComp("FileOpenRecent.comp");

        Console.WriteLine("App successfully shut down.");
    }
}