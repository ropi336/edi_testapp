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
internal class ModifyExistingAndSave
{
    public static void Run()
    {
        string ediPath = TestPaths.EdiPath;

        string filePath = TestPaths.TestFile("Modify.txt");
        string filePathBackup = TestPaths.BackupFile("Modify.txt");

        using var edi = new TestManager(ediPath);

        string cExpectedFileContents = "ModifyLineTwoWithSevenSevens\r\n7777777";
        string cExpectedUnmodifiedContents = "ModifyLineTwoWithSevenSevens";

        const string cPopUpOpenButtonId = "1";
        const string cPopUpSaveButton = "1";

        edi.Launch();
        edi.CloseAllDocuments();

        Console.WriteLine($"Preparing test case prerequisites.");

        File.Copy(filePathBackup, filePath, true);

        Thread.Sleep(1000);

        edi.OpenFile(filePath, cPopUpOpenButtonId);

        Thread.Sleep(400);

        Console.WriteLine($"Commencing test.");

        edi.LogResult("Ensure document is unmodified.", cExpectedUnmodifiedContents, edi.GetDocumentContents());

        Console.WriteLine($"Skipping to next line.");

        Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.END);
        Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);

        Console.WriteLine($"Modifying file with 7 sevens.");

        for (int i = 0; i < 7; i++)
        {
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_7);
            Thread.Sleep(50);
        }

        edi.LogResult("Save file changes.", true, edi.InvokeButtonByName("Save"));

        Thread.Sleep(1000);

        string actualContents = edi.GetFileContents(filePath);

        edi.LogResult("Compare file contents after Save action.", cExpectedFileContents, actualContents);

        edi.LogResult("Compare document contents after Save action.", cExpectedFileContents, edi.GetDocumentContents());

        Thread.Sleep(1000);

        edi.CloseAllDocuments();

        edi.Close();

        Console.WriteLine("App successfully shut down.");
    }
}