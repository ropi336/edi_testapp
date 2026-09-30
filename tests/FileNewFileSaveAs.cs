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

        edi.CompletePopUpProcedure("Save As", filePath);

        AutomationElement saveAsWindow = edi.FindWindowByName("Save As");

        //Click Save
        edi.InvokeButtonInWindow(saveAsWindow, "1");

        Thread.Sleep(300);

        //Check for overwrite prompt, then click yes if exists
        if (edi.CheckOverwritePrompt(saveAsWindow) == true)
        {
            edi.InvokeButtonInWindow(saveAsWindow, "6");
        }

        Console.ReadLine();

        if (edi.FileExists(filePath))
        {
            Console.WriteLine("[PASS] New file was successfully created.");
        }
        else
        {
            Console.WriteLine("[FAIL] The file was not created.");
        }


        Thread.Sleep(1000);

        edi.Close();

        Console.WriteLine("App successfully shut down.");
    }
}
