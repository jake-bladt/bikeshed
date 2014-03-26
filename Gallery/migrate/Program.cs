using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageGallery;

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

            return false;
        }
    }
}
