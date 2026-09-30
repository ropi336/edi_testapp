using System.Text.RegularExpressions;
using System.Windows.Forms;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using Microsoft.VisualBasic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestApp.Tests;
internal class UndoRedo
{
    public static void Run()
    {
        string ediPath = TestPaths.EdiPath;

        string filePath = TestPaths.TestFile("UndoRedo.txt");

        using var edi = new TestManager(ediPath);

        string ExpectedFileContents = "";

        const string cPopUpOpenButtonId = "1";

        edi.LogResult("Launching EDI", true, edi.Launch());

        Console.WriteLine($"Preparing test case prerequisites.");

        edi.CloseAllDocuments();

        edi.OpenFile(filePath, cPopUpOpenButtonId);

        Thread.Sleep(400);

        Console.WriteLine($"Commencing test.");

        edi.LogResult("Expand 'Edit' Menu item", true, edi.ExpandMenuItemByName("Edit"));

        edi.LogResult("Invoke 'Select All' Menu item", true, edi.InvokeMenuItemByName("Select All"));

        edi.LogResult("Expand 'Edit' Menu item", true, edi.ExpandMenuItemByName("Edit"));

        edi.LogResult("Invoke 'Delete' Menu item", true, edi.InvokeMenuItemByName("Delete"));

        edi.LogResult("Compare document contents after delete action.", ExpectedFileContents, edi.GetDocumentContents());

        edi.LogResult("Expand 'Edit' Menu item", true, edi.ExpandMenuItemByName("Edit"));

        edi.LogResult("Invoke 'Undo' Menu item", true, edi.InvokeMenuItemByName("Undo"));

        Thread.Sleep(300);

        ExpectedFileContents = "DeleteThisStringOfText";

        edi.LogResult("Compare document contents after Undo action.", ExpectedFileContents, edi.GetDocumentContents());

        edi.LogResult("Expand 'Edit' Menu item", true, edi.ExpandMenuItemByName("Edit"));

        edi.LogResult("Invoke 'Undo' Menu item", true, edi.InvokeMenuItemByName("Redo"));

        Thread.Sleep(300);

        ExpectedFileContents = "";

        edi.LogResult("Compare document contents after Redo action.", ExpectedFileContents, edi.GetDocumentContents());

        edi.LogResult("Expand 'File' Menu item", true, edi.ExpandMenuItemByName("File"));

        edi.LogResult("Invoke 'Save' Menu item", true, edi.InvokeMenuItemByName("Save"));

        Console.WriteLine("Saving file to disk..");

        Thread.Sleep(3000);

        string actualContents = edi.GetFileContents(filePath);

        edi.LogResult("Compare file contents after save", "", actualContents);

        Thread.Sleep(1000);

        edi.CloseAllDocuments();

        edi.Close();

        Console.WriteLine("App successfully shut down.");
    }
}