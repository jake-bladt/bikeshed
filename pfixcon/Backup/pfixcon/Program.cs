using System;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Text;

namespace pfixcon
{
    class Program
    {
        static void Main(string[] args)
        {
            if (1 == args.Length)
            {
                var dirPath = args[0];
                if(Directory.Exists(dirPath))
                {
                    var di = new DirectoryInfo(dirPath);
                    var jpegs = di.GetFiles(@"*.jpg");
                    var rnd = new Random();

                    foreach (var jpg in jpegs)
                    {
                        var fullName = jpg.FullName;
                        var fileName = jpg.Name;
                        var prefix = rnd.NextDouble() * 999999.0;
                        var newName = prefix.ToString("000000") + fileName;
                        Console.WriteLine(String.Format("Renaming {0} to {1}", fileName, newName));
                        File.Move(fullName, di.FullName + Path.DirectorySeparatorChar + newName);
                    }

                }
                else
                {
                    Console.WriteLine(String.Format("Directory {0} does not exist.", dirPath));
                }
            }
            else
            {
                Console.WriteLine(@"pfixcon requires a path passed as an argument.");
            }
            Console.ReadLine();
        }
    }
}
