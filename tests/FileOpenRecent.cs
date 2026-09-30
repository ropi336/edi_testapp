using System.Windows.Forms;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestApp.Tests;
internal class FileOpenRecent
{
    public static void Run()
    {
        string ediPath = @"D:\CPROJECT\Edi-1.2\Debug\Edi.exe";

        string filePath = "D:\\CPROJECT\\edi_testapp\\edi_testapp\\testapp_savedir\\OpenRecent.txt";

        using var edi = new TestManager(ediPath);

        const string cExpectedFileContents = "OpenRecent";

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

        Console.WriteLine("[PASS] Expanded 'Open' Menu Item.");

        if (!edi.ExpandMenuItemByName("Recent Documents"))
        {
            Console.WriteLine("[FAIL] 'Recent Documents' Menu Item was not found.");
            return;
        }

        Console.WriteLine("[PASS] Expanded 'Recent Documents' Menu Item.");

        if (!edi.OpenFirstRecentDocument())
        {
            Console.WriteLine("[FAIL] Failed to open most recent document.");
            return;
        }

        Console.WriteLine("[PASS] Opened most recent document successfully.");

        Thread.Sleep(200);

        if (cExpectedFileContents == edi.GetDocumentContents())
        {
            Console.WriteLine("[PASS] Opened file contents match expected result.");
        }
        else
        {
            Console.WriteLine("[FAIL] Opened file contents did not match expected result.");
        }

        Thread.Sleep(1000);

        edi.Close();

        Console.WriteLine("App successfully shut down.");
    }
}