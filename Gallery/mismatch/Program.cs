using System;
using System.Configuration;
using System.Linq;
using System.IO;

namespace mismatch
{
    class Program
    {
        static void Main(string[] args)
        {
            var subjectRoot = ConfigurationManager.AppSettings["subjectRoot"];
            var subjectsDI = new DirectoryInfo(subjectRoot);
            var subjectDirs = subjectsDI.GetDirectories();

            var yearbookRoot = ConfigurationManager.AppSettings["yearbookRoot"];
            var yearbookDI = new DirectoryInfo(yearbookRoot);
            var yearbookFiles = yearbookDI.GetFiles("*.jpg");
            var yearbookSubjectNames = yearbookFiles.ToList().Select(f => f.Name.Replace(".jpg", String.Empty)).ToList();

            subjectDirs.ToList().ForEach(dir => {
                if (!yearbookSubjectNames.Contains(dir.Name)) Console.WriteLine(dir.Name);
            });
            
            Console.ReadLine();

        }
    }
}
