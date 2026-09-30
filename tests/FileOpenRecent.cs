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

        Thread.Sleep(200);

        edi.CloseAllDocuments();

        Thread.Sleep(200);

        Console.WriteLine($"Commencing test.");

        edi.LogResult("Expand 'File' menu item", true, edi.ExpandMenuItemByName("File"));

        edi.LogResult("Expand 'Open' menu item", true, edi.ExpandMenuItemByName("Open"));

        edi.LogResult("Expand recent documents menu item", true, edi.ExpandMenuItemByName("Recent Documents"));

        edi.LogResult("Open most recent document.", true, edi.OpenFirstRecentDocument());

        Thread.Sleep(200);

        edi.LogResult("Compare document contents after opening document.", cExpectedFileContents, edi.GetDocumentContents());

        Thread.Sleep(1000);

        edi.Close();

        Console.WriteLine("App successfully shut down.");
    }
}