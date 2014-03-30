using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageGallery.Subjects;

namespace ImageGallery
{
    public interface IImageGallery
    {
        ISubject Subject(string name);
        Dictionary<String, ISubject> Subjects { get; }
    }
}
