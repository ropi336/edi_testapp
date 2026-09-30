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
internal class CutAndPaste
{
    public static void Run()
    {
        string ediPath = @"D:\CPROJECT\Edi-1.2\Debug\Edi.exe";

        string filePath = "D:\\CPROJECT\\edi_testapp\\edi_testapp\\testapp_savedir\\Cut.txt";
        
        string newFilePath = "D:\\CPROJECT\\edi_testapp\\edi_testapp\\testapp_savedir\\NewFilePaste.txt";

        using var edi = new TestManager(ediPath);

        string cExpectedFileContents = "CutThisStringOfText";
        string cExpectedEmptyContents = "";

        const string cPopUpOpenButtonId = "1";
        const string cPopUpSaveButton = "1";

        edi.LogResult("Launching EDI", true, edi.Launch());

        Console.WriteLine($"Preparing test case prerequisites.");

        edi.CloseAllDocuments();
        edi.ExpandMenuItemByName("File");
        edi.ExpandMenuItemByName("Open");
        edi.InvokeMenuItemByName("Text files");

        Thread.Sleep(200);

        edi.CompletePopUpProcedure("Open", filePath);

        Thread.Sleep(200);

        AutomationElement openWindow = edi.FindWindowByName("Open");

        edi.InvokeButtonInWindow(openWindow, cPopUpOpenButtonId);

        Thread.Sleep(400);

        Console.WriteLine($"Commencing test.");

        edi.LogResult("Expand 'Edit' Menu item", true, edi.ExpandMenuItemByName("Edit"));

        edi.LogResult("Invoke 'Select All' Menu item", true, edi.InvokeMenuItemByName("Select All"));

        edi.LogResult("Expand 'Edit' Menu item", true, edi.ExpandMenuItemByName("Edit"));

        edi.LogResult("Invoke 'Cut' Menu item", true, edi.InvokeMenuItemByName("Cut"));

        edi.LogResult("Compare document contents after Cut action.", cExpectedEmptyContents, edi.GetDocumentContents());

        edi.LogResult("Expand 'File' Menu item", true, edi.ExpandMenuItemByName("File"));

        edi.LogResult("Expand 'New' Menu item", true, edi.ExpandMenuItemByName("New"));

        edi.LogResult("Invoke 'Text Document' Menu item", true, edi.InvokeMenuItemByName("Text Document"));

        Thread.Sleep(400);

        edi.LogResult("Expand 'Edit' Menu item", true, edi.ExpandMenuItemByName("Edit"));

        edi.LogResult("Invoke 'Paste' Menu item", true, edi.InvokeMenuItemByName("Paste"));

        edi.LogResult("Compare document contents after Paste action.", cExpectedFileContents, edi.GetDocumentContents());

        edi.LogResult("Expand 'File' menu item", true, edi.ExpandMenuItemByName("File"));

        edi.LogResult("Invoke 'Save As' Menu item", true, edi.InvokeMenuItemByName("Save As"));

        Thread.Sleep(300);

        edi.LogResult("Input file path to Save As popup", true, edi.CompletePopUpProcedure("Save As", newFilePath));

        AutomationElement saveAsWindow = edi.FindWindowByName("Save As");

        //Click Save
        edi.LogResult("Invoke Save Button", true, edi.InvokeButtonInWindow(saveAsWindow, cPopUpSaveButton));

        Console.WriteLine("Saving file to disk..");

        Thread.Sleep(1800);

        string actualContents = edi.GetFileContents(newFilePath);

        edi.LogResult("Compare file contents after save", cExpectedFileContents, actualContents);

        Thread.Sleep(1000);

        edi.Close();

        Console.WriteLine("App successfully shut down.");

        //After test cleanup
        edi.DeleteFile(newFilePath);
    }
}