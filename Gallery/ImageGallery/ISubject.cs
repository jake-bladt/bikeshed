using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallery
{
    public interface ISubject
    {
        string Name { get; }
        string DisplayName { get;  }
        int ImageCount { get; }
    }
}
