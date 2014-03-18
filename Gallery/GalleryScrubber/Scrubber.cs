using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageGallery;

namespace GalleryScrubber
{
    public class Scrubber
    {
        protected ImageGallery.ImageGallery _gallery;
        public Scrubber(ImageGallery.ImageGallery gallery) 
        { 
            _gallery = gallery;  
        }
    }
}
