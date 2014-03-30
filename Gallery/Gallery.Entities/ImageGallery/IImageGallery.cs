using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.ImageGallery
{
    public interface IImageGallery
    {
        ISubject Subject(string name);
        Dictionary<String, ISubject> Subjects { get; }
    }
}
