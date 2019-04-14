using System;
using System.Configuration;
using System.IO;

using Gallery.Entities.Candidates;
using Gallery.Entities.ImageGallery;

namespace specdir
{
    class Program
    {
        static void Main(string[] args)
        {
            string setName = args[0];
            string cn = GetDbConnectionString();
            string poolRoot = ConfigurationManager.AppSettings["yearbookLocation"];
            string specialRoot = Path.Combine(ConfigurationManager.AppSettings["electionsRoot"], "special");

            var gallery = new SqlTrackedImageGallery(cn);
            var pool = CandidatePool.FromGallery(gallery);

            var chooser = new SetCandidateChooser(setName, cn) { Name = setName };
            var choosers = new ICandidateChooser[] { chooser };
            var registrar = new ContestCandidateRegistrar(pool, choosers);
            var candidateSet = registrar.GetContestCandidates();
            var writer = new FileSystemContestWriter(specialRoot, poolRoot);
            var msg = writer.WriteContests(candidateSet) ? "Write complete." : "Write failed.";
            Console.WriteLine(msg);
            Console.ReadLine();
        }

        private static string GetDbConnectionString()
        {
            var env = Environment.GetEnvironmentVariable("GALLERY_CONSTR");
            return String.IsNullOrEmpty(env) ?
                ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString : env;
        }

    }
}
