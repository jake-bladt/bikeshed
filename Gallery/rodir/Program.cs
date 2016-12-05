using System;
using System.Configuration;
using System.IO;

using Gallery.Entities.Candidates;
using Gallery.Entities.ImageGallery;

namespace rodir
{
    class Program
    {
        static void Main(string[] args)
        {
            var roCountString = args[0];
            int contestantCount = Int32.Parse(args[1]);
            var sourceList = args.Length > 2 ? args[2] : String.Empty;

            string cn = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;
            string poolRoot = ConfigurationManager.AppSettings["yearbookLocation"];
            string roRoot = Path.Combine(ConfigurationManager.AppSettings["electionsRoot"], "runoff");

            var gallery = new SqlTrackedImageGallery(cn);
            var pool = CandidatePool.FromGallery(gallery);

            CandidatePool subsetPool;
            if(String.IsNullOrEmpty(sourceList))
            {
                ICandidateChooser chooser = null;
                chooser = new AllCandidateChooser(pool) { Name = sourceList };
                var choosers = new ICandidateChooser[] { chooser };
                var registrar = new ContestCandidateRegistrar(pool, choosers);
                var candidateSets = registrar.GetContestCandidates();
                var subset = candidateSets[sourceList];
                subsetPool = CandidatePool.FromListOfSubjects(subset);
            }
            else
            {
                subsetPool = pool;
            }

            // TODO - Convert the subset to a pool.
            // The number of cycles through the pool is either rocCount or infinite.
            // Loop until the number of cycles is depleted or all candidates in the subset
            // have been assigned to a runoff.
            // Write those runoffs to disc.

        }
    }
}
