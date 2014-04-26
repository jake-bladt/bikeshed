using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.Candidates
{
    public class ContestCandidateRegistrar
    {
        protected ICandidatePool _Pool;
        protected ICandidateChooser[] _Choosers;

        public ContestCandidateRegistrar(ICandidatePool pool, ICandidateChooser[] choosers)
        {
            _Pool = pool;
            _Choosers = choosers;
        }

        public Dictionary<String, List<ISubject>> GetContestCandidates()
        {
            var ret = new Dictionary<String, List<ISubject>>();

            for (int i = 0; i < _Choosers.Length; i++)
            {
                var chooser = _Choosers[i];
                var actual = new List<ISubject>();
                var allPossible = chooser.GetCandidates();
                allPossible.ForEach(subj =>
                {
                    if (_Pool.HasSubject(subj.Name) != null)
                    {
                        actual.Add(subj);
                        _Pool.Remove(subj);
                    }
                });
                ret[chooser.Name] = actual;
            }

            return ret;
        }

    }
}
