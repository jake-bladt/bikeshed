using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JakeBladt.TimeBlocks
{
    public enum TimeBlockIDGenerationStrategy
    {
        HumanReadable,
        SequentialFromEpoch,
        Custom,
        NoIDSupport
    }
    public class TimeBlockFactory<T> where T: TimeBlockBase
    {

        protected TimeBlockIDGenerationStrategy IDStrategy;
        protected DateTime Epoch;
        protected Func<Int64, DateTime, DateTime> CustomIDStrategy;

        public TimeBlockFactory(
            TimeBlockIDGenerationStrategy idStrategy, 
            DateTime? epoch = null, 
            Func<Int64, DateTime, DateTime> customIDStrategy = null)
        {
            IDStrategy = idStrategy;
            Epoch = (DateTime)(epoch.HasValue ? epoch.Value : DateTime.MinValue);
            CustomIDStrategy = customIDStrategy;
        }

        public T GetTimeblock(DateTime seedDt)
        {
            

            throw new NotImplementedException();
        }
    }
}
