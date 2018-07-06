using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.Candidates;
using Gallery.Entities.Subjects;

namespace sarro
{
    class Program
    {
        static void Main(string[] args)
        {
            var runoffRoot = args[0];
            var targetCount = args.Length > 1 ? Int32.Parse(args[1]) : 30;

            var cnStr = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;
            var chooser = new SuperannuatedRookieChooser(cnStr);
            var sars = chooser.GetCandidates();
            Console.WriteLine($"{sars.Count} SARs discovered.");

            var pool = CandidatePool.FromListOfSubjects(sars);

            Console.ReadLine();
        }
    }
}
