using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.Candidates
{
    public class AllCandidateChooser : ICandidateChooser
    {
        protected ICandidatePool _Pool;
        public AllCandidateChooser(ICandidatePool pool)
        {
            _Pool = pool;
        }

        public List<ISubject> GetCandidate()
        {
            var ret = new List<ISubject>();

            

            return ret;
        }
    }
}
