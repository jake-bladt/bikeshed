using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gallery.Entities.Subjects;

namespace Gallery.Entities.Candidates
{
    public class ProsepctCandidateChooser : ICandidateChooser
    {
        public string Name => throw new NotImplementedException();

        public List<ISubject> GetCandidates()
        {
            throw new NotImplementedException();
        }
    }
}
