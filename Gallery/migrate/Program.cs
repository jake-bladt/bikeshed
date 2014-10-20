using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.Elections;
using Gallery.Entities.ImageGallery;
using Gallery.Migration;

namespace migrate
{
    class Program
    {
        static int Main(string[] args)
        {
            var subjectResult = MigrateSubjects();
            if (!subjectResult) return ExitOn("Subject migration failed.");

            var electionResult = MigrateElections();
            if (!electionResult) return ExitOn("Election migration failed.");

            return ExitOn("Migration complete.");
        }

        public static int ExitOn(string step)
        {
            Console.WriteLine(step);
            Console.ReadLine();
            return 0;
        }

        public static bool MigrateSubjects()
        {
            var connStr = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;
            var dbGallery = new SqlTrackedImageGallery(connStr);
            int dbCount = dbGallery.Subjects.Count;
            Console.WriteLine(String.Format("{0} subjects in the database.", dbCount.ToString("#,##0")));

            var galleryPath = ConfigurationManager.AppSettings["gallerySource"].ToString();
            var fsoGallery = new FileSystemImageGallery(galleryPath);
            int fsCount = fsoGallery.Subjects.Count;
            Console.WriteLine(String.Format("{0} subjects in the file system.", fsCount.ToString("#,##0")));

            var res = SubjectMigrationHelper.MigrateToDB(fsoGallery, dbGallery);
            Console.WriteLine(String.Format("{0} subject(s) successfully migrated.", res.Saved));
            Console.WriteLine(String.Format("{0} subject(s) failed to migrate.", res.Failures));

            return true;
        }

        public static bool MigrateElections()
        {
            string rootPath = ConfigurationManager.AppSettings["electionSource"];

            var connStr = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;
            var dbGallery = new SqlTrackedImageGallery(connStr);
            var targetSet = new SqlBackedElectionSet(connStr);

            var helper = new ElectionMigrationHelper(dbGallery, targetSet);
            return helper.MigrateHistory(rootPath);
        }

        public static bool MigrateSubjectSets()
        {

            return false;
        }
    }
}
