using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;

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

            var walkinChooser = new WalkInCandidateChooser(pool, 250.0) { Name = "walkin" };
            var rookieChooser = new RookieCandidateChooser(cn) { Name = "rookie" };
            var travelChooser = new AllCandidateChooser(pool) { Name = "travel" };
            var choosers = new ICandidateChooser[] { walkinChooser, rookieChooser, travelChooser };
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
