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

        using var edi = new TestManager(ediPath);
        
        if (!edi.Launch())
        {
            Console.WriteLine($"Could not launch EDI.");
        }

        Console.WriteLine($"Launched EDI.");

        if (!edi.InvokeButton("New"))
        {
            Console.WriteLine("[FAIL] New button was not found.");
            return;
        }

        Console.WriteLine("[PASS] Pressed 'New' button.");

        var untitled = edi.FindByName("Untitled.txt");

        if (untitled == null)
        {
            Console.WriteLine("[FAIL] Untitled.txt was not found.");
            return;
        }

        Console.WriteLine("[PASS] Untitled.txt has been created.");

        if (!edi.InvokeButton("SaveAs"))
        {
            Console.WriteLine("[FAIL] Save As button was not found.");
            return;
        }

        Console.WriteLine("[PASS] Save As invoked.");

        Thread.Sleep(300);

        AutomationElement saveAsWindow = edi.FindWindowByName("Save As");

        AutomationElement fileNameBox = edi.FindFileNameInput(saveAsWindow);

        var textBox = fileNameBox.AsTextBox();

        if (textBox != null)
        {
            textBox.Text = "D:\\CPROJECT\\edi_testapp\\edi_testapp\\testapp_savedir\\Untitled.txt";
        }

        //edi.PrintTree();

        //Click Save
        edi.InvokeButtonInWindow(saveAsWindow, "1");

        Thread.Sleep(300);

        //Check for overwrite prompt, then click yes if exists
        if (edi.CheckOverwritePrompt(saveAsWindow) == true)
        {
            edi.InvokeButtonInWindow(saveAsWindow, "6");
        }

        Console.ReadLine();

        edi.Close();
    }
}
