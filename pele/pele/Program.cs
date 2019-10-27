using System;
using System.IO;

namespace pele
{
    class Program
    {
        public static string Path { get; set; }

        public static void Usage()
        {
            Console.WriteLine("Usage: pele <directory-to-watch>");
        }

        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Usage();
                return;
            }

            Path = args[0];
            var watcher = new FileSystemWatcher(Path);


            Console.ReadLine();
        }
    }
}
