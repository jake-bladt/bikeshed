using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallery
{
    public class FileSystemImageGallery : IImageGallery
    {
        protected string _root;
        protected Dictionary<string, FileBackedSubject> _subjects;

        public FileSystemImageGallery(string rootPath)
        {
            if (!Directory.Exists(rootPath)) throw new FileNotFoundException(
                 String.Format("There is no directory named {0}.", rootPath));
            _root = rootPath;
        }

        public FileBackedSubject Subject(string name)
        {
            var subjectPath = Path.Combine(_root, name);
            if (!Directory.Exists(subjectPath)) throw new ArgumentException(String.Format("There is no subject named {0}.", name));
            return new FileBackedSubject(subjectPath, name);
        }

        public Dictionary<string, FileBackedSubject> Subjects
        {
            get
            {
                if(null == _subjects)
                {
                    _subjects = new Dictionary<string, FileBackedSubject>();
                    var di = new DirectoryInfo(_root);
                    var subjectDirs = di.GetDirectories("*", SearchOption.TopDirectoryOnly);
                    subjectDirs.ToList().ForEach(sdi => _subjects[sdi.Name] = new FileBackedSubject(sdi.FullName, sdi.Name));
                }
                return _subjects;
            }
        }

    }
}
