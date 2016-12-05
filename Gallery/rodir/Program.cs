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

            ICandidateChooser chooser = null;
            if(String.IsNullOrEmpty(sourceList))
            {
                chooser = new AllCandidateChooser(pool) { Name = sourceList };
            }
            else
            {
                chooser = new SetCandidateChooser(sourceList, cn) { Name = sourceList };
            }
            var choosers = new ICandidateChooser[] { chooser };
            var registrar = new ContestCandidateRegistrar(pool, choosers);
            var candidateSets = registrar.GetContestCandidates();
            var subset = candidateSets[sourceList];



        }
    }
}
