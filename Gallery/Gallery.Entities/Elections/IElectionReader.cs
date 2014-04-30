using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Entities.Elections
{
    public interface IElectionReader
    {
        List<IElection> GetAllElections();
        IElection GetElection(int id);
    }
}
