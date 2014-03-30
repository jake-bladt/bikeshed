using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Entities.Elections
{
    public interface IElectionSet
    {
        bool Store(Election election);
    }
}
