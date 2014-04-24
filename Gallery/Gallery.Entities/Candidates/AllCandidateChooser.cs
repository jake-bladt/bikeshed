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

        public List<ISubject> GetCandidates()
        {
            var ret = new List<ISubject>();
            _Pool.Candidates.ForEach(c => ret.Add(c));
            return ret;
        }
    }
}
