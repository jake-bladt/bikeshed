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
                var fileMask = args.Length > 1 ? args[1] : "*.*";

                if(Directory.Exists(dirPath))
                {
                    var di = new DirectoryInfo(dirPath);
                    var targetFiles = di.GetFiles(@"*.jpg");
                    var rnd = new Random();

                    foreach (var tFile in targetFiles)
                    {
                        var fullName = tFile.FullName;
                        var fileName = tFile.Name;
                        var prefix = rnd.NextDouble() * 499999.0 +500000;
                        var newName = "x" + prefix.ToString("000000") + fileName;
                        Console.WriteLine(String.Format("Renaming {0} to {1}", fileName, newName));
                        File.Move(fullName, Path.Combine(di.FullName, newName));
                    }
                }
                else
                {
                    Console.WriteLine(String.Format("Directory {0} does not exist.", dirPath));
                }
            }
            else
            {
                Console.WriteLine("pfixcon requires a path passed as an argument.");
            }
            Console.ReadLine();
        }
    }
}
