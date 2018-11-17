using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace pfixrnd
{
    class Program
    {
        static void Main(string[] args)
        {
            var reversalFlag = (args[0] == "-r");
            var dirPath = args[0] == "-r" ? args[1] : args[0]; 
            var di = new DirectoryInfo(dirPath);
            var fis = di.GetFiles("*.jpg");
            string pattern = @"^x\d{5}-";
            var random = new Random();

            fis.ToList().ForEach(fi =>
            {
                var regexMatch = Regex.IsMatch(fi.Name, pattern);
                if (regexMatch && reversalFlag)
                {

                }
                else if(!regexMatch && !reversalFlag)
                {
                    int prefixNum = random.Next(0, 50000) + 50000;
                    var newFileName = String.Format("x{0}-{1}", prefixNum, fi.Name);
                    var newFilePath = Path.Combine(di.FullName, newFileName);
                    Console.WriteLine(String.Format("{0} -> {1}", fi.Name, newFileName));
                    File.Move(fi.FullName, newFilePath);
                }
            });

            Console.ReadLine();

        }
    }
}
