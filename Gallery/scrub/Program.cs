using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageGallery;

namespace scrub
{
    class Program
    {
        static void Main(string[] args)
        {
            var galleryPath = ConfigurationManager.AppSettings["GallerySource"];
            var gallery = new ImageGallery.ImageGallery(galleryPath);



            Console.ReadLine();
        }
    }
}
