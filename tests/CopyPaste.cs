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
internal class CopyPaste
{
    public static void Run()
    {
        string ediPath = @"D:\CPROJECT\Edi-1.2\Debug\Edi.exe";

        string filePath = TestPaths.TestFile("CopyPaste.txt");

        string filePathBackup = TestPaths.BackupFile("CopyPaste.txt");

        File.Copy(filePathBackup, filePath, true);

        using var edi = new TestManager(ediPath);

        string cExpectedFileContents = "CopyThisString\r\nCopyThisString";

        const string cPopUpOpenButtonId = "1";

        Console.WriteLine($"Preparing test case prerequisites.");

        edi.LogResult("Launching EDI", true, edi.Launch());

        edi.CloseAllDocuments();

        edi.OpenFile(filePath, cPopUpOpenButtonId);

        Thread.Sleep(400);

        Console.WriteLine($"Commencing test.");

        edi.LogResult("Expand 'Edit' Menu item", true, edi.ExpandMenuItemByName("Edit"));

        edi.LogResult("Invoke 'Select All' Menu item", true, edi.InvokeMenuItemByName("Select All"));

        edi.LogResult("Expand 'Edit' Menu item", true, edi.ExpandMenuItemByName("Edit"));

        edi.LogResult("Invoke 'Copy' Menu item", true, edi.InvokeMenuItemByName("Copy"));

        //Keyboard operation to move one line down.
        Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.RIGHT);
        Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);

        edi.LogResult("Expand 'Edit' Menu item", true, edi.ExpandMenuItemByName("Edit"));

        edi.LogResult("Invoke 'Paste' Menu item", true, edi.InvokeMenuItemByName("Paste"));

        edi.LogResult("Compare document contents after paste action.", cExpectedFileContents, edi.GetDocumentContents());

        edi.LogResult("Expand 'File' Menu item", true, edi.ExpandMenuItemByName("File"));

        edi.LogResult("Invoke 'Save' Menu item", true, edi.InvokeMenuItemByName("Save"));

        Console.WriteLine("Saving file to disk..");

        Thread.Sleep(1800);

        string actualContents = edi.GetFileContents(filePath);

        edi.LogResult("Compare file contents after save", cExpectedFileContents, actualContents);

        Thread.Sleep(1000);

        edi.CloseAllDocuments();

        edi.Close();

        Console.WriteLine("App successfully shut down.");
    }
}