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
            Console.WriteLine(String.Format("{0} subjects in database.", dbCount));

            return false;
        }
    }
}
