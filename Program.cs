using System.Diagnostics;

namespace Edi.TestApp;

internal class Program
{
    static void Main()
    {
        string ediPath = @"D:\CPROJECT\Edi-1.2\Debug\Edi.exe";

        Console.WriteLine("Starting Edi");

        var process = Process.Start(ediPath);

        if (process == null)
        {
            Console.WriteLine("FAIL: Could not start Edi.");
            return;
        }

        Console.WriteLine($"Edi started. PID: {process.Id}\nWaiting for program to be ready.");

        process.WaitForInputIdle();

        Console.WriteLine("Edi is ready.");
        Console.WriteLine($"Window handle: {process.MainWindowHandle}");

        Console.WriteLine();
        Console.WriteLine("Press ENTER to close Edi.");
        Console.ReadLine();

        process.CloseMainWindow();

        if (!process.WaitForExit(5000))
        {
            Console.WriteLine("Edi did not close within 5 seconds.");
        }
        else
        {
            Console.WriteLine("Edi closed successfully.");
        }
    }
}