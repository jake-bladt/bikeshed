using System;
using System.Configuration;
using System.IO;

using Gallery.Entities.Elections;
using Gallery.Entities.ImageGallery;
using Gallery.Migration;

namespace migrate
{
    class Program
    {
        static int Main(string[] args)
        {
            var parts = args.Length > 0 ? args[0] : "*";

            if(parts == "*" || parts == "categories")
            {
                Console.WriteLine("Migrating categories");
                var cateResult = MigrateCategories();
                if (!cateResult) return ExitOn("Category migration failed.");
            }

            if (parts == "*" || parts == "subjects")
            {
                Console.WriteLine("Migrating subjects.");
                var subjectResult = MigrateSubjects();
                if (!subjectResult) return ExitOn("Subject migration failed.");
            }

            if (parts == "*" || parts == "elections")
            {
                Console.WriteLine("Migrating elections.");
                var electionResult = MigrateElections();
                if (!electionResult) return ExitOn("Election migration failed.");
            }

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
            if (!helper.MigrateHistory(rootPath)) return false;
            var specs = helper.MigrateSpecials(rootPath);
            var ret = helper.MigrateRunoffs(rootPath) && specs;
            return ret;
        }

        public static bool MigrateCategories()
        {
            var connStr = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;
            var helper = new CategoryMigrationHelper(connStr);
            var rootDir = ConfigurationManager.AppSettings["galleryRoot"];

            var outputPath = Path.Combine(rootDir, "cates.txt");
            var writer = new CategoryMigrationWriter(outputPath);
            var importCount = helper.Migrate((l) => writer.WriteExportLine(l));
            Console.WriteLine($"{importCount.ToString("#,##0")} subject categories migrated.");
            return true;
        }

        public static bool MigrateSubjectSets()
        {

            return false;
        }
    }
}
