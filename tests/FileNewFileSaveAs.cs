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

        int testPasses = 0;

        using var edi = new TestManager(ediPath);

        const string cOverridePopUpYesButtonId = "6";
        const string cSaveAsPopUpSaveButtonId = "1";
        
        if (!edi.Launch())
        {
            Console.WriteLine($"Could not launch EDI.");
        }
        Console.WriteLine($"Launched EDI.");

        if (!edi.ExpandMenuItemByName("File"))
        {
            Console.WriteLine("[FAIL] 'File' Menu Item was not found.");
            return;
        }
        Console.WriteLine("[PASS] Expanded 'File' Menu Item.");

        if (!edi.ExpandMenuItemByName("New"))
        {
            Console.WriteLine("[FAIL] 'New' Menu Item was not found.");
            return;
        }
        Console.WriteLine("[PASS] Expanded 'New' Menu Item.");

        if (!edi.InvokeMenuItemByName("Text Document"))
        {
            Console.WriteLine("[FAIL] 'Text Document' Menu Item was not found.");
            return;
        }
        Console.WriteLine("[PASS] Invoked 'Text Document' Menu item.");

        Thread.Sleep(100);

        var untitled = edi.FindByName("Untitled.txt");

        if (untitled == null)
        {
            Console.WriteLine("[FAIL] Untitled.txt was not found.");
            return;
        }
        Console.WriteLine("[PASS] Untitled.txt has been created.");

        if (!edi.ExpandMenuItemByName("File"))
        {
            Console.WriteLine("[FAIL] File Menu Item was not found.");
            return;
        }
        Console.WriteLine("[PASS] Expanded 'File' Menu Item.");

        if (!edi.InvokeMenuItemByName("Save As"))
        {
            Console.WriteLine("[FAIL] 'Save As' Menu Item was not found.");
            return;
        }
        Console.WriteLine("[PASS] Invoked 'Save As' item.");

        Thread.Sleep(300);

        if (!edi.CompletePopUpProcedure("Save As", filePath))
        {
            Console.WriteLine("[FAIL] Path could not be added to 'Save As' popup.");
        }
        Console.WriteLine("[PASS] Path was successfully added to 'Save As' popup.");

        AutomationElement saveAsWindow = edi.FindWindowByName("Save As");

        //Click Save
        edi.InvokeButtonInWindow(saveAsWindow, cSaveAsPopUpSaveButtonId);

        Thread.Sleep(300);

        //Check for overwrite prompt, then click yes if exists
        if (edi.CheckOverwritePrompt(saveAsWindow) == true)
        {
            edi.InvokeButtonInWindow(saveAsWindow, cOverridePopUpYesButtonId);
        }

        if (edi.FileExists(filePath))
        {
            Console.WriteLine("[PASS] New file was successfully saved to storage.");
        }
        else
        {
            Console.WriteLine("[FAIL] The file could not be saved to storage.");
        }

        Thread.Sleep(1000);

        edi.Close();

        Console.WriteLine("App successfully shut down.");
    }
}
