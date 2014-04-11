using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace ss2dir
{
    class Program
    {
        static void Main(string[] args)
        {
            string ssName = args[0];
            string targetPath = args[1];
            string line;
            string ssPath = ConfigurationManager.AppSettings["slideshowLocation"];
            string showPath = Path.Combine(ssPath, ssName + ".txt");
            StreamReader rdr = new StreamReader(showPath);
            while((line = rdr.ReadLine()) != null)
            {
                var fi = new FileInfo(line);
                var targetLocation = Path.Combine(targetPath, fi.Name);
                Console.WriteLine(String.Format("{0} => {1}", fi.FullName, targetLocation));
                File.Copy(fi.FullName, targetLocation);
            }
            Console.ReadLine();
        }
    }
}
