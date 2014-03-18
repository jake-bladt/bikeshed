using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallery
{
    public class Subject
    {
        public string DirectoryPath { get; protected set; }
        public string Name { get; protected set; }

        public Subject(string path, string name)
        {
            DirectoryPath = path;
            Name = name;
        }
    }
}
