using System;
using System.Configuration;

using Gallery.Entities.Candidates;
using Gallery.Entities.ImageGallery;

namespace specdir
{
    class Program
    {
        static void Main(string[] args)
        {
            string contestPath = args[0];
            string setName = args[1];

            string cn = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;
            string poolRoot = ConfigurationManager.AppSettings["yearbookLocation"];

            var gallery = new SqlTrackedImageGallery(cn);
            var pool = CandidatePool.FromGallery(gallery);

            var chooser = new SetCandidateChooser(args[1], cn) { Name = setName };
            var choosers = new ICandidateChooser[] { chooser };
            var registrar = new ContestCandidateRegistrar(pool, choosers);
            var candidateSet = registrar.GetContestCandidates();
            var writer = new FileSystemContestWriter(contestPath, poolRoot);
            var msg = writer.WriteContests(candidateSet) ? "Write complete." : "Write failed.";
            Console.WriteLine(msg);
            Console.ReadLine();
        }
    }
}
