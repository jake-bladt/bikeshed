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
            
            var gallery = new SqlTrackedImageGallery(cn);
            var pool = CandidatePool.FromGallery(gallery);

            var rookieChooser = new RookieCandidateChooser(cn) { Name = "rookie" };
            var travelChooser = new AllCandidateChooser(pool) { Name = "travel" };
            var choosers = new ICandidateChooser[] { rookieChooser, travelChooser };
            var registrar = new ContestCandidateRegistrar(pool, choosers);
            var candidateSets = registrar.GetContestCandidates();

            candidateSets.ToList().ForEach(kvp =>
            {
                Console.WriteLine("Set: " + kvp.Key);
                kvp.Value.ForEach(subj =>
                {
                    Console.WriteLine(subj.DisplayName);
                });
            });

            Console.ReadLine();
        }
    }
}
