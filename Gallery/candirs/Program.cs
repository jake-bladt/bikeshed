using System;
using System.Configuration;

using Gallery.Entities.Candidates;
using Gallery.Entities.ImageGallery;

namespace candirs
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = args[0];
            string cn = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;
            string poolRoot = ConfigurationManager.AppSettings["yearbookLocation"];
            
            var gallery = new SqlTrackedImageGallery(cn);
            var pool = CandidatePool.FromGallery(gallery);

            var travelChooser = new SetCandidateChooser(cn, args[1]) { Name = "travel" };
            var walkinChooser = new WalkInCandidateChooser(pool, 300.0) { Name = "walkin" };
            var rookieChooser = new RookieCandidateChooser(cn) { Name = "rookie" };
            var starChooser = new AllCandidateChooser(pool) { Name = "star" };
            var choosers = new ICandidateChooser[] { travelChooser, walkinChooser, rookieChooser, starChooser };
            var registrar = new ContestCandidateRegistrar(pool, choosers);
            var candidateSets = registrar.GetContestCandidates();
            var writer = new FileSystemContestWriter(rootPath, poolRoot);
            if (writer.WriteContests(candidateSets))
            {
                Console.WriteLine("Contests written.");
            }
            else
            {
                Console.WriteLine("Failed to write contests.");
            }

            Console.ReadLine();
        }
    }
}
