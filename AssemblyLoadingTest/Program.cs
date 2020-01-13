using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyLoadingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load assembly from plugin folder

            string path = Directory.GetCurrentDirectory();
            string pluginPath = Path.Combine(path, "plugin");
            var pluginDirectories = Directory.GetDirectories(pluginPath);


            var allDlls = new List<FileInfo>();

            foreach (var dir in pluginDirectories)
            {
                var files = Directory.GetFiles(dir).Select(x => new FileInfo(x)).ToList();
                allDlls.AddRange(files.Where(x => x.Extension == ".dll").ToList());
            }

            //Distinct and get latest version.
            allDlls = allDlls.GroupBy(x => x.Name).Select(x => x.OrderByDescending(element => new Version(FileVersionInfo.GetVersionInfo(element.FullName).FileVersion)).First()).ToList();

            //Invoker dll:
            var invokerDlls = allDlls.Where(x => x.Name.Contains(x.Directory.Name)).ToList();

            List<IMyInterface> invokerList = new List<IMyInterface>();


            AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
            {
                Console.WriteLine($"Assembly Resolving from {e.RequestingAssembly.FullName}, looking for {e.Name}.");

                var assemblyName = e.Name.Split(',')[0];
                var tmp = allDlls.FirstOrDefault(x => x.Name.Contains(assemblyName));
                return Assembly.LoadFile(tmp.FullName);
            };

            foreach (var dll in invokerDlls)
            {
                var assembly = Assembly.LoadFile(dll.FullName);
                Type type = assembly.GetExportedTypes().First();

                var instanceOfMyType = Activator.CreateInstance(type) as IMyInterface;
                Console.WriteLine(instanceOfMyType.GetSecretString());
                
            }

            Console.ReadLine();
        }
    }
}
