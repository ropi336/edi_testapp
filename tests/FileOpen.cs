using System.Windows.Forms;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestApp.Tests;
internal class FileOpen
{
    public static void Run()
    {
        string ediPath = @"D:\CPROJECT\Edi-1.2\Debug\Edi.exe";

        using var edi = new TestManager(ediPath);

        string filePath = "D:\\CPROJECT\\edi_testapp\\edi_testapp\\testapp_savedir\\TextContent.txt";

        const string cSaveAsPopUpOpenButtonId = "1";

        const string cExpectedFileContents = "SampleText";

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

        if (!edi.ExpandMenuItemByName("Open"))
        {
            Console.WriteLine("[FAIL] 'Open' Menu Item was not found.");
            return;
        }

        Console.WriteLine("[PASS] Invoked 'Open' item.");

        if (!edi.InvokeMenuItemByName("Text files"))
        {
            Console.WriteLine("[FAIL] 'Text files' Menu Item was not found.");
            return;
        }

        Console.WriteLine("[PASS] Invoked 'Text files' item.");

        // edi.PrintTree();

        Thread.Sleep(300);

        edi.CompletePopUpProcedure("Open", filePath);

        AutomationElement openWindow = edi.FindWindowByName("Open");

        if (!edi.InvokeButtonInWindow(openWindow, cSaveAsPopUpOpenButtonId))
        {
            Console.WriteLine("[FAIL] Could not Invoke 'Open' Button.");
            return;
        }
        Console.WriteLine("[PASS] Successfully Invoked 'Open' Button.");

        Thread.Sleep(200);

        edi.PrintEditableTextBoxContents("PART_EditableTextBox");

        Thread.Sleep(1000);

        edi.Close();
    }
}