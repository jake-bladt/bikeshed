using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.Candidates
{
    public interface ICandidateChooser
    {
        List<ISubject> GetCandidates();
        string Name { get; }
    }
}
