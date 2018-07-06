using System;
using System.Configuration;
using System.IO;
using System.Linq;

using Gallery.Entities.Candidates;
using Gallery.Entities.Subjects;

namespace runoffs
{
    class Program
    {
        static void Main(string[] args)
        {
            var runoffRoot = args[0];
            var targetCount = Int32.Parse(args[1]);
            var contestCount = args.Length > 2 ? Int32.Parse(args[2]) : 1;
            
            string poolRoot = ConfigurationManager.AppSettings["yearbookLocation"];
            var poolInfo = new DirectoryInfo(poolRoot);
            var pool = CandidatePool.FromFileSystemDirectory(poolInfo);
            var chooser = new WalkInCandidateChooser(pool, targetCount * contestCount);

            var candidates = chooser.GetCandidates().Shuffle();
            var candidatesArray = candidates.ToArray();
            for (int i = 1; i <= contestCount; ++i)
            {
                var runoffPath = Path.Combine(runoffRoot, i.ToString("000"));
                Directory.CreateDirectory(runoffPath);
                for (int j = 1; j <= targetCount; ++j)
                {
                    var targetCandidateSlot = (i - 1) * targetCount + j - 1;
                    if (candidatesArray.Length > targetCandidateSlot)
                    {
                        var candy = (FsoBackedSubject)candidatesArray[targetCandidateSlot];
                        var targetLocation = Path.Combine(runoffPath, candy.DisplayName);
                        Console.WriteLine(String.Format("Copying {0} to {1}", candy.Name, targetLocation));
                        File.Copy(candy.FullPath, targetLocation);
                    }
                }
            }

            Console.ReadLine();

        }
    }
}
