using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MDK_UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly string SpaceEngineersDirectory;

        public App()
        {
            SpaceEngineersDirectory = new Malware.MDKUtilities.SpaceEngineers().GetInstallPath();

            if (string.IsNullOrWhiteSpace(SpaceEngineersDirectory))
            {
                MessageBox.Show("Unable to locate Space Engineers installation.", "Missing requirement", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Environment.Exit(-2);
            }

            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var args = e.Args;
            var program = args.FirstOrDefault();

            var main = new MainWindow(program);
            main.Show();
        }

        Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var directories = new string[]
            {
                Path.Combine(SpaceEngineersDirectory, "Bin64"),
                Environment.CurrentDirectory,
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            };

            var assemblyName = new AssemblyName(args.Name).Name;

            Debug.WriteLine($"Resolving Dynamic dependency: {assemblyName}.");

            string assemblyNameDll = assemblyName + ".dll";
            string assemblyNameExe = assemblyName + ".exe";

            string assemblyPathToUse = "";
            foreach (var directory in directories)
            {
                string assemblyPathDll = Path.Combine(directory, assemblyNameDll);
                string assemblyPathExe = Path.Combine(directory, assemblyNameExe);

                if (File.Exists(assemblyPathDll))
                {
                    assemblyPathToUse = assemblyPathDll;
                    break;
                }
                else if (File.Exists(assemblyPathExe))
                {
                    assemblyPathToUse = assemblyPathExe;
                    break;
                }
            }

            if (!string.IsNullOrWhiteSpace(assemblyPathToUse) && File.Exists(assemblyPathToUse))
            {
                return Assembly.LoadFrom(assemblyPathToUse);
            }

            return null;
        }
    }
}
