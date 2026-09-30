using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    internal static class TestPaths
    {
        public static string TestFilesDirectory =>
            Path.Combine(
                AppContext.BaseDirectory,
                "testapp_savedir");
        public static string BackupsDirectory =>
            Path.Combine(AppContext.BaseDirectory,
                "testapp_testapp_backupfiles");
        public static string CompFilesDirectory =>
            Path.Combine(AppContext.BaseDirectory,
                "testapp_compfiles");
        public static string TestFile(string fileName) =>
            Path.Combine(
                TestFilesDirectory,
                fileName);
    }
}