using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallery
{
    public interface IImageGallery
    {
        FileBackedSubject Subject(string name);
        Dictionary<String, FileBackedSubject> Subjects { get; }
    }
}
