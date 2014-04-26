using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.Candidates
{
    public interface ICandidatePool
    {
        ISubject HasSubject(string name);
        bool Add(ISubject subject);
        bool Remove(ISubject subject);
        List<ISubject> Candidates { get; }
    }
}
