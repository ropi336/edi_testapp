using System.Diagnostics;
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

        Console.WriteLine("Printing all UI Elements");

        var saveAsWindow = edi.MainWindow.FindFirstDescendant(
        cf => cf.ByControlType(ControlType.Window).And(cf.ByName("Save As")));

        //edi.PrintTree(saveAsWindow);

        AutomationElement addressToolbar = edi.FindAddressToolbar(saveAsWindow);

        var patterns = addressToolbar.GetSupportedPatterns();

        foreach (var pattern in patterns)
        {
            Console.WriteLine(pattern);
        }

        Console.ReadLine();

        Thread.Sleep(5000);

        edi.Close();
    }
}
