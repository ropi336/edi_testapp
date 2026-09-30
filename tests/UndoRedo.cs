using System.Windows.Forms;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestApp.Tests;
internal class UndoRedo
{
    public static void Run()
    {
        string ediPath = @"D:\CPROJECT\Edi-1.2\Debug\Edi.exe";

        string filePath = "D:\\CPROJECT\\edi_testapp\\edi_testapp\\testapp_savedir\\UndoRedo.txt";

        using var edi = new TestManager(ediPath);

        const string cExpectedFileContents = "SampleText";

        const string cPopUpOpenButtonId = "1";

        if (!edi.Launch())
        {
            Console.WriteLine($"Could not launch EDI.");
        }
        Console.WriteLine($"Launched EDI.");

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

        Thread.Sleep(200);

        edi.CloseAllDocuments();

        Thread.Sleep(200);

        Console.WriteLine($"Commencing test.");

        Console.ReadLine();
        
        Thread.Sleep(1000);

        edi.Close();

        Console.WriteLine("App successfully shut down.");
    }
}