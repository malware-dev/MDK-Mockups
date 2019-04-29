#define UseMDK
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataExtractor
{
    static partial class Program
    {
#if UseMDK
        const string RegistryKey = "HKEY_CURRENT_USER\\Software\\Malware\\MDK";
        const string RegistryValue = "SEBinPath";
        const string RootRelative = "..";
#else
        const string RegistryKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 244850";
        const string RegistryValue = "InstallLocation";
        const string RootRelative = ".";
#endif

        const string OutputPath = "..\\..\\..\\Mockups\\Base";

        public static async Task<int> Main(string[] args)
        {
            var sePath = args.ElementAtOrDefault(0);
            var output = args.ElementAtOrDefault(1);

            if (string.IsNullOrWhiteSpace(sePath))
                sePath = Registry.GetValue(RegistryKey, RegistryValue, "")?.ToString();

            if (string.IsNullOrWhiteSpace(output))
                output = OutputPath;

            if (string.IsNullOrWhiteSpace(sePath))
            {
#if UseMDK
                Console.WriteLine("Malware's Dev Kit is not installed.");
#else
                Console.WriteLine("Space Engineers is not installed.");
#endif
                Console.ReadKey(true);
                return -1;
            }

            sePath = Path.Combine(sePath, RootRelative);

            var binary = Path.Combine(sePath, "Bin64\\SpaceEngineers.exe");
            var version = FileVersionInfo.GetVersionInfo(binary);
            var modified = File.GetLastWriteTime(binary);
            var headerBuilder = new StringBuilder();
            headerBuilder.AppendLine("///");
            headerBuilder.AppendLine("/// This file is auto generated based on the Space Engineers content directory.");
            headerBuilder.AppendLine($"/// Space Engineers version: {version.FileVersion}, date: {modified}");
            headerBuilder.AppendLine("/// ");
            headerBuilder.AppendLine();

            var header = headerBuilder.ToString();

            await GenerateTextSurfaceValues(sePath, output, header);

            return 0;
        }
    }
}
