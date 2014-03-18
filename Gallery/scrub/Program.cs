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
            var galleryPath = ConfigurationManager.AppSettings["GallerySource"];
            var gallery = new ImageGallery.ImageGallery(galleryPath);
            var scrubber = new Scrubber(gallery);
            

            Console.ReadLine();
        }
    }
}
