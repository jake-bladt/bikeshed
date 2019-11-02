using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Entities.Elections
{
    public class ElectionResultSet : List<SingleElectionResult>
    {
        public bool Has(SingleElectionResult toMatch)
        {
            return this.Any<SingleElectionResult>(r =>
                (r.ElectionId == toMatch.ElectionId || r.ElectionName == toMatch.ElectionName) &&
                (r.SubjectId == toMatch.SubjectId || r.SubjectName == toMatch.SubjectName) &&
                r.OrdinalRank == toMatch.OrdinalRank
            );
        }
    }
}
