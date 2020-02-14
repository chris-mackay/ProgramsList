using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace ProgramsList
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IEnumerable<string> list = GetFiles(@"c:\", "*.exe");
                
                Console.WriteLine("Getting all .exe files on c:\\...");
                Console.WriteLine("Total items: " + list.Count());
                Console.WriteLine();

                Console.WriteLine("Writing file...");

                StreamWriter writer = new StreamWriter(Environment.UserName + "-proglist.csv");
                writer.WriteLine("Filename,ProductName,Version");

                foreach (string item in list)
                {
                    FileVersionInfo vInfo = FileVersionInfo.GetVersionInfo(item);
                    FileInfo fInfo = new FileInfo(item);

                    if (!item.ToUpper().Contains(@"C:\WINDOWS"))
                    {
                        if(vInfo.ProductName != null &&
                           vInfo.ProductVersion != null)
                        {
                            string version = vInfo.FileMajorPart + "." + 
                                             vInfo.FileMinorPart + "." + 
                                             vInfo.FileBuildPart + "." + 
                                             vInfo.FilePrivatePart;

                            writer.WriteLine(fInfo.Name + "," +
                                             vInfo.ProductName + "," +
                                             version);
                        }
                    }
                }

                Console.WriteLine("Completed. Please close the window...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        public static IEnumerable<string> GetFiles(string root, string searchPattern)
        {
            Stack<string> pending = new Stack<string>();
            pending.Push(root);
            while (pending.Count != 0)
            {
                var path = pending.Pop();
                string[] next = null;
                try
                {
                    next = Directory.GetFiles(path, searchPattern);
                }
                catch { }
                if (next != null && next.Length != 0)
                    foreach (var file in next) yield return file;
                try
                {
                    next = Directory.GetDirectories(path);
                    foreach (var subdir in next) pending.Push(subdir);
                }
                catch { }
            }
        }
    }
}
