using System.Diagnostics;
using TestApp.Tests;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;

namespace TestApp;

internal class Program
{
    static void Main()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();

            Console.WriteLine("EDI AUTOMATED TEST MANAGER");
            Console.WriteLine("==========================");
            Console.WriteLine();
            Console.WriteLine("1. File - New File - Save As");
            Console.WriteLine("2. File - Open");
            Console.WriteLine("3. File - Open Recent");
            Console.WriteLine("4. Copy - Paste");
            Console.WriteLine("5. Cut from file and paste into a new file");
            Console.WriteLine("6. Undo - Redo");
            Console.WriteLine("7. Automatically Open Recents");
            Console.WriteLine("8. Close A document");
            Console.WriteLine("9. Modify and save an existing document");
            Console.WriteLine("10. Open multiple files - Switch tabs");
            Console.WriteLine("0. Exit");
            Console.WriteLine();

            Console.Write("Select test: ");
            string? selection = Console.ReadLine();

            Console.WriteLine();

            switch (selection)
            {
                case "1":
                    FileNewFileSaveAs.Run();
                    break;

                case "2":
                    FileOpen.Run();
                    break;

                case "3":
                    FileOpenRecent.Run();
                    break;
                    
                case "4":
                    CopyPaste.Run();
                    break;

                case "5":
                    CutAndPaste.Run();
                    break;
                    
                case "6":
                    UndoRedo.Run();
                    break;
                    
                case "7":
                    AutoOpenRecents.Run();
                    break;

                case "8":
                    CloseDocument.Run();
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }

            if (running)
            {
                Console.WriteLine();
                Console.WriteLine("Press ENTER to return to test manager.");
                Console.ReadLine();
            }
        }
    }
}