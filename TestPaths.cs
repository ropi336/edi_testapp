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
        public static string BackupFilesDirectory =>
            Path.Combine(AppContext.BaseDirectory,
                "testapp_backupfiles");
        public static string CompFilesDirectory =>
            Path.Combine(AppContext.BaseDirectory,
                "testapp_compfiles");
        public static string TestFile(string fileName) =>
            Path.Combine(
                TestFilesDirectory,
                fileName);
        public static string BackupFile(string fileName) =>
            Path.Combine(
                BackupFilesDirectory,
                fileName);
        public static string CompFile(string fileName) =>
            Path.Combine(
                CompFilesDirectory,
                fileName);

        public static string EdiPath =>
            Path.GetFullPath(
                Path.Combine(
                    AppContext.BaseDirectory,
                    @"..\..\..\..\..\Edi-1.2\Debug\Edi.exe"));
    }
}