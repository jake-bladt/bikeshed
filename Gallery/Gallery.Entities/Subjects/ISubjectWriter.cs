using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Entities.Subjects
{
    public interface ISubjectWriter
    {
        bool UpdateImageCount(string name, int imageCount);
    }
}
