using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallery
{
    public class Subject
    {
        public string DirectoryPath { get; protected set; }
        public string Name { get; protected set; }

        protected Dictionary<String, FileInfo> _files;

        public Subject(string path, string name)
        {
            DirectoryPath = path;
            Name = name;
        }

        public Dictionary<String, FileInfo> Files
        {
            get
            {
                if(null == _files)
                {
                    _files = new Dictionary<string, FileInfo>();
                    var di = new DirectoryInfo(DirectoryPath);
                    var arr = di.GetFiles("*.jpg", SearchOption.AllDirectories);
                    arr.ToList().ForEach(fi => _files[fi.FullName] = fi);
                }
                return _files;
            }
        }
    }
}
