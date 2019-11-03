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
        public IImageGallery Gallery { get; protected set; }

        public ElectionMigrationHelper(IImageGallery gallery)
        {
            Gallery = gallery;
        }

        public List<SingleElectionResult> GetDeltas(ElectionResultSet setFrom, ElectionResultSet setTo)
        {
            return setFrom.Where(r => !setTo.Has(r)).ToList();
        }

        public bool ApplyDeltas(List<SingleElectionResult> deltas)
        {
            var effectedElectionNames = deltas.GroupBy(d => d.ElectionName).Select(grp => grp.First().ElectionName);
            effectedElectionNames.ToList().ForEach(Console.WriteLine);
            return true;
        }

    }
}
