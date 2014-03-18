using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallery
{
    public class ImageGallery
    {
        protected string _root;

        public ImageGallery(string rootPath)
        {
            if (!Directory.Exists(rootPath)) throw new FileNotFoundException(
                 String.Format("There is no directory named {0}.", rootPath));
            _root = rootPath;
        }
    }
}
