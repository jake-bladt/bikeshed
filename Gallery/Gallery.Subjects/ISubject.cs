using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Subjects
{
    public interface ISubject
    {
        int ID { get; }
        string Name { get; }
        string DisplayName { get;  }
        int ImageCount { get; }
    }
}

