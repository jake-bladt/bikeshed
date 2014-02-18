using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JakeBladt.TimeBlocks
{
    public class MonthBlock : TimeBlockBase
    {
        public MonthBlock(DateTime seed) : base(seed) { }

        protected override DateTime GetFirstInstant(DateTime seed)
        {
            var month = seed.Month;
            var year = seed.Year;
            return new DateTime(year, month, 1);
        }

        protected override DateTime GetFinishingBoundaryInstant(DateTime seed)
        {
            var start = GetFirstInstant(seed);
            return start.AddMonths(1);
        }
    }
}
