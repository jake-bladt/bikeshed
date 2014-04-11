using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.ImageGallery;
using Gallery.Entities.Subjects;

namespace scrub
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var galleryPath = ConfigurationManager.AppSettings["GallerySource"];
                var gallery = new FileSystemImageGallery(galleryPath);
                var dbCnStr = ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString;

                var dbUpdater = new SqlSubjectWriter(dbCnStr);
                var scrubber = new Scrubber(gallery, dbUpdater);

                if (args.Length > 0)
                {
                    string target = args[0].ToLower();
                    var ret = scrubber.ScrubSubject(target);
                    Console.WriteLine(ret);
                }
                else
                {
                    var ret = scrubber.Scrub();
                    ret.ForEach(l => Console.WriteLine(l));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
