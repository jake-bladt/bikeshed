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

            // Presume we always have a category rider for now.
            var riderCategory = args[2].Replace(".", " ");
            var riderChooser = new RiderCategoryChooser(riderCategory, cn) { Name = "rider" };

            var travelChooser = new SetCandidateChooser(args[1], cn) { Name = "travel" };
            var prospectChooser = new ProspectCandidateChooser(cn) { Name = "prospect" };
            var walkinChooser = new WalkInCandidateChooser(pool, 300.0) { Name = "walkin" };
            var rookieChooser = new RookieCandidateChooser(cn) { Name = "rookie" };
            var starChooser = new StarChooser(cn) { Name = "star" };
            var choosers = new ICandidateChooser[] { riderChooser, travelChooser, prospectChooser, walkinChooser, rookieChooser, starChooser };
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
