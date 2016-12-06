using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;

using Gallery.Entities.Candidates;
using Gallery.Entities.ImageGallery;
using Gallery.Entities.Subjects;

namespace rodir
{
    class Program
    {
        static void Main(string[] args)
        {
            var roSetName = args[0];
            var roCountString = args[1];
            int contestantCount = Int32.Parse(args[2]);
            var sourceList = args.Length > 3 ? args[3] : String.Empty;

            string cn = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;
            string poolRoot = ConfigurationManager.AppSettings["yearbookLocation"];
            string roRoot = Path.Combine(ConfigurationManager.AppSettings["electionsRoot"], "runoff");

            var gallery = new SqlTrackedImageGallery(cn);
            var pool = CandidatePool.FromGallery(gallery);

            CandidatePool subsetPool = pool;
            if(!String.IsNullOrEmpty(sourceList))
            {
                ICandidateChooser chooser = null;
                chooser = new AllCandidateChooser(pool) { Name = sourceList };
                var choosers = new ICandidateChooser[] { chooser };
                var registrar = new ContestCandidateRegistrar(pool, choosers);
                var candidateSets = registrar.GetContestCandidates();
                var subset = candidateSets[sourceList];
                subsetPool = CandidatePool.FromListOfSubjects(subset);
            }

            // The number of cycles through the pool is either rocCount or infinite.
            var runCount = roCountString == "*" ? Int32.MaxValue : Int32.Parse(roCountString);

            // Loop until the number of cycles is depleted or all candidates in the subset
            // have been assigned to a runoff.
            var runoffSets = new Dictionary<string, List<ISubject>>();
            for(int i = 1; i <= runCount; i++)
            {
                var chooserName = String.Format("runoff{0}", i.ToString("000"));
                var runoffChooser = new WalkInCandidateChooser(pool, contestantCount) { Name = chooserName };
                var subsetChoosers = new ICandidateChooser[] { runoffChooser };
                var subsetRegistrar = new ContestCandidateRegistrar(pool, subsetChoosers);
                var runoffCandidateSets =subsetRegistrar.GetContestCandidates();
                var runoffCandidates = runoffCandidateSets[chooserName];
                if(runoffCandidates.Count > 0)
                {
                    runoffSets[i] = runoffCandidates;
                }
                else
                {
                    break;
                }
            }

            // Write the runoffs to disc.

        }

        static string OrdinalToRunoffName(int ordinal)
        {
            return String.Format("{0}-{1}", DateTime.Now.ToString("yyyyMMdd"), ordinal.ToString("###"));
        }
    }
}
