using System.Diagnostics;
using System.Windows.Forms;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestApp.Tests;
internal class FileNewFileSaveAs
{
    public static void Run()
    {
        string ediPath = @"D:\CPROJECT\Edi-1.2\Debug\Edi.exe";

        string filePath = "D:\\CPROJECT\\edi_testapp\\edi_testapp\\testapp_savedir\\Untitled.txt";

        using var edi = new TestManager(ediPath);

        const string cOverridePopUpYesButtonId = "6";
        const string cPopUpSaveButton = "1";

        edi.DeleteFile(filePath);

        edi.LogResult("Launching EDI", true, edi.Launch());

        edi.CloseAllDocuments();

        edi.LogResult("Expand 'File' menu item", true, edi.ExpandMenuItemByName("File"));

        edi.LogResult("Expand 'New' menu item", true, edi.ExpandMenuItemByName("New"));

        edi.LogResult("Invoke 'Text Document' Menu item", true, edi.InvokeMenuItemByName("Text Document"));

        Thread.Sleep(100);

        //Check available tabs if new file exists
        edi.LogResult("Search for any tab named Untitled.txt", true, edi.TabExists("Untitled.txt"));

        edi.LogResult("Expand 'File' menu item", true, edi.ExpandMenuItemByName("File"));

        edi.LogResult("Invoke 'Save As' Menu item", true, edi.InvokeMenuItemByName("Save As"));

        Thread.Sleep(300);

        edi.LogResult("Input file path to Save As popup", true, edi.CompletePopUpProcedure("Save As", filePath));

        AutomationElement saveAsWindow = edi.FindWindowByName("Save As");

        //Click Save
        edi.LogResult("Invoke Save Button", true, edi.InvokeButtonInWindow(saveAsWindow, cPopUpSaveButton));

        Thread.Sleep(300);

        //Check for overwrite prompt, then click yes if exists.
        //Always skip the overwrite prompt silently.
        if (edi.CheckOverwritePrompt(saveAsWindow) == true)
        {
            edi.InvokeButtonInWindow(saveAsWindow, cOverridePopUpYesButtonId);
        }

        Console.WriteLine("Saving File...");

        Thread.Sleep(500);

        edi.LogResult("Checking saved file status", true, edi.FileExists(filePath));

        edi.CloseAllDocuments();

        Thread.Sleep(1000);

        edi.Close();

        Console.WriteLine("App successfully shut down.");
    }
}
