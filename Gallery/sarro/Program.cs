using System;
using System.Configuration;
using System.IO;
using System.Linq;

using Gallery.Entities.Candidates;

namespace sarro
{
    class Program
    {
        static void Main(string[] args)
        {
            var runoffRoot = args[0];
            var targetCount = args.Length > 1 ? Int32.Parse(args[1]) : 18;
            var yearbookHome = ConfigurationManager.AppSettings["yearbookLocation"];

            var cnStr = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;
            var chooser = new SuperannuatedRookieChooser(cnStr);
            var sars = chooser.GetCandidates();
            Console.WriteLine($"{sars.Count} SARs discovered.");

            var rnd = new Random();
            var shuffled = sars.OrderBy(x => rnd.Next()).ToArray();
            var roNum = 0;
            DirectoryInfo roDi = null;
            for(int i = 0; i < shuffled.Length; i++)
            {
                if(i == 0 || i % targetCount == 0)
                {
                     var roPath = $"{runoffRoot}{(++roNum).ToString("000")}";
                    roDi = Directory.CreateDirectory(roPath);
                }
                var fn = $"{shuffled[i].Name}.jpg";
                var srcPath = Path.Combine(yearbookHome, fn);
                var tarPath = Path.Combine(roDi.FullName, fn);
                if (File.Exists(srcPath))
                {
                    Console.WriteLine($"Copying {srcPath} to {tarPath}.");
                    // File.Copy(srcPath, tarPath);
                }
            }

            Console.WriteLine("Operation complete.");
            Console.ReadLine();
        }
    }
}
