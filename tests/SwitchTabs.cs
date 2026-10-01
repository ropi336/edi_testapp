using System.Text.RegularExpressions;
using System.Windows.Forms;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using Microsoft.VisualBasic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

//======================
// Switch Tabs test
//=======================
// Purpose: Verify that switching tabs correctly functions
// by, after switching, checking file contents against the data on record
//=======================

namespace TestApp.Tests;
internal class SwitchTabs
{
    public static void Run()
    {
        using var edi = new TestManager(TestPaths.EdiPath);

        string filePathCopyPaste = TestPaths.BackupFile("CopyPaste.txt");
        string filePathCut = TestPaths.BackupFile("Cut.txt");
        string filePathOpenRecent = TestPaths.BackupFile("Modify.txt");

        const string cExpectedDocContentsCopyPaste = "CopyThisString";
        const string cExpectedDocContentsCut = "CutThisStringOfText";
        const string cExpectedDocContentsModify = "ModifyLineTwoWithSevenSevens";

        const string cPopUpOpenButtonId = "1";

        edi.Launch();

        Console.WriteLine($"Preparing test case prerequisites.");

        Console.WriteLine($"Closing all documents.");

        edi.CloseAllDocuments();

        Console.WriteLine($"Opening files CopyPaste.txt, Cut.txt, and Modify.txt.");

        edi.OpenFile(filePathCopyPaste, cPopUpOpenButtonId);
        edi.OpenFile(filePathCut, cPopUpOpenButtonId);
        edi.OpenFile(filePathOpenRecent, cPopUpOpenButtonId);

        Console.WriteLine($"Commencing test.");

        Thread.Sleep(500);

        edi.LogResult("Select tab CopyPaste.txt", true, edi.SelectTabByName("CopyPaste.txt"));

        Thread.Sleep(500);

        edi.LogResult("Verify document contents after Tab switching action.", cExpectedDocContentsCopyPaste, edi.GetDocumentContents());

        edi.LogResult("Select tab Cut.txt", true, edi.SelectTabByName("Cut.txt"));

        Thread.Sleep(500);

        edi.LogResult("Verify document contents after Tab switching action.", cExpectedDocContentsCut, edi.GetDocumentContents());

        edi.LogResult("Select tab Modify.txt", true, edi.SelectTabByName("Modify.txt"));

        Thread.Sleep(500);

        edi.LogResult("Verify document contents after Tab switching action.", cExpectedDocContentsModify, edi.GetDocumentContents());

        edi.CloseAllDocuments();

        edi.Close();

        edi.CompareWithComp("SwitchTabs.comp");

        Console.WriteLine("App successfully shut down.");
    }
}