using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gallery.Entities.Subjects;

namespace Gallery.Entities.Candidates
{
    public class WalkInCandidateChooser : ICandidateChooser 
    {
        protected ICandidatePool _pool;
        protected double _targetCount;

        public WalkInCandidateChooser(ICandidatePool pool, double targetCount)
        {
            _pool = pool;
            _targetCount = targetCount;
        }

        public List<ISubject> GetCandidates()
        {
            var ret = new List<ISubject>();
            var chance = _pool.Candidates.Count() / _targetCount;
            var rng = new Random();
            _pool.Candidates.ForEach(c =>
            {
                if (chance < rng.NextDouble()) ret.Add(c);
            });
            return ret;
        }

        public string Name { get; set; }
    }
}
