using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageGallery;
using ImageGallery.Migration;

namespace migrate
{
    class Program
    {
        static void Main(string[] args)
        {
            var subjectResult = MigrateSubjects();

            Console.ReadLine();
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
    }
}
