namespace AquilaService
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.Versioning;

    public static class FileHelper
    {
        /// <summary>
        /// Check file name whether valid
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns><list type="bullet">
        /// <item><term>True</term><description>Valid</description></item>
        /// <item><term>False</term><description>Otherwise</description></item>
        /// </list></returns>
        public static bool IsValidFileName(string fileName)
        {
            // Check if the file name is null or empty
            if (string.IsNullOrEmpty(fileName))
                return false;

            // Check if the file name contains any invalid characters
            char[] invalidChars = Path.GetInvalidFileNameChars();
            if (fileName.IndexOfAny(invalidChars) >= 0)
                return false;

            // Check if the file name starts or ends with a space or period
            if (fileName.StartsWith(" ") || fileName.EndsWith(" ") ||
                fileName.StartsWith(".") || fileName.EndsWith("."))
                return false;

            // Check if the file name is a reserved device name on Windows
            string[] reservedNames = { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4",
                               "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2",
                               "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9", };
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if (reservedNames.Contains(fileNameWithoutExtension.ToUpperInvariant()))
                return false;

            // Additional checks can be added based on your requirements
            return true;
        }

        /// <summary>
        /// Get path of Application
        /// </summary>
        /// <param name="productName">Name Application</param>
        /// <returns>All paths</returns>
        [SupportedOSPlatform("windows")]
        public static List<string>? GetAppPath(string productName)
        {
            const string foldersPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\Folders";
            var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);

            var subKey = baseKey.OpenSubKey(foldersPath);
            if (subKey == null)
            {
                baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                subKey = baseKey.OpenSubKey(foldersPath);
            }

            return subKey?.GetValueNames().Where(kv => kv.Contains(productName)).ToList();
        }

        /// <summary>
        /// Get all files from root directory
        /// </summary>
        /// <param name="root">directory</param>
        /// <param name="searchPattern">search pattern</param>
        /// <returns>All path of files</returns>
        public static IEnumerable<string> GetFiles(string root, string searchPattern)
        {
            Stack<string> pending = new();
            pending.Push(root);
            while (pending.Count != 0)
            {
                var path = pending.Pop();
                string[]? next = null;
                try
                {
                    next = Directory.GetFiles(path, searchPattern);
                }
                catch
                {
                }

                if (next != null && next.Length != 0)
                {
                    foreach (var file in next)
                        yield return file;
                }

                try
                {
                    next = Directory.GetDirectories(path);
                    foreach (var subdir in next)
                        pending.Push(subdir);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Check if EA.exe:
        /// <list type="bullet">
        /// <item>Is exist</item>
        /// <item>Version = 16.x</item>
        /// </list>
        /// </summary>
        /// <param name="pathEA">path to EA File</param>
        /// <returns>True if satisfy above conditions</returns>
        public static bool CheckEA(string? pathEA)
        {
            if (pathEA is null || !System.IO.File.Exists(pathEA))
                return false;

            var versionInfo = FileVersionInfo.GetVersionInfo(pathEA);
            var version = versionInfo.FileVersion?.Split(", ").FirstOrDefault();
            var intVersion = int.Parse(version);
            //if (version is not null && (version == "16" || version == "15"))
            if (version is not null && intVersion >= 15)
                return true;

            return false;
        }
    }
}
