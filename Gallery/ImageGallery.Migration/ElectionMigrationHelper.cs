using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Gallery.Entities.Elections;
using Gallery.Entities.ImageGallery;
using Gallery.Entities.Utilities;

namespace Gallery.Migration
{
    public class ElectionMigrationHelper
    {
        public IElectionWriter Writer { get; protected set; }

        public ElectionMigrationHelper(IElectionWriter writer)
        {
            Writer = writer;
        }

        public List<SingleElectionResult> GetDeltas(ElectionResultSet setFrom, ElectionResultSet setTo)
        {
            return setFrom.Where(r => !setTo.Has(r)).ToList();
        }

        public bool ApplyDeltas(List<SingleElectionResult> deltas)
        {
            var effectedElectionTops = deltas.GroupBy(d => d.ElectionName).Select(grp => grp.First(w => w.OrdinalRank == 1));
            effectedElectionTops.ToList().ForEach(e =>
            {
                var electionResults = deltas.Where(d => d.ElectionName == e.ElectionName).ToList();
                var electionId = Writer.Upsert(e);
                Writer.AddWiners(electionId, electionResults);
            });
            return true;
        }

    }
}
