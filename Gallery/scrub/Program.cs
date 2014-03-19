using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageGallery;
using GalleryScrubber;

namespace scrub
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var galleryPath = ConfigurationManager.AppSettings["GallerySource"];
                var gallery = new ImageGallery.ImageGallery(galleryPath);
                var scrubber = new Scrubber(gallery);

                if (args.Length > 0)
                {
                    string target = args[0].ToLower();
                    var ret = scrubber.ScrubSubject(target);
                    Console.WriteLine(ret);
                }
                else
                {

                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }
    }
}
