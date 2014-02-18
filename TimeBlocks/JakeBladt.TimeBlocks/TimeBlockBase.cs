using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JakeBladt.TimeBlocks
{
    public abstract class TimeBlockBase : ITimeBlock
    {
        public DateTime SeedDate { get; protected set; }
        public DateTime FirstInstant { get; protected set; }
        public DateTime FinishingBoundaryInstant { get; protected set; }
        public long ID  { get; protected set; }

        public TimeBlockBase(DateTime seed, 
            TimeBlockIDGenerationStrategy idStrategy, 
            DateTime? epoch = null, 
            Func<DateTime, DateTime, Int64> customIDStrategy = null)
        {
            SeedDate = seed;
            FirstInstant = GetFirstInstant(seed);
            FinishingBoundaryInstant = GetFinishingBoundaryInstant(seed);

            switch (idStrategy)
            {
                case TimeBlockIDGenerationStrategy.HumanReadable:
                    ID = GetHumanReadableID(FirstInstant, FinishingBoundaryInstant);
                    break;
                case TimeBlockIDGenerationStrategy.SequentialFromEpoch:
                    ID = GetSequentialFromEpochID(FirstInstant, epoch);
                    break;
                case TimeBlockIDGenerationStrategy.Custom:
                    ID = GetCustomID(customIDStrategy);
                    break;
                default:
                    ID = (Int64)GetHashCode();
                    break;
            }

        }

        abstract protected DateTime GetFirstInstant(DateTime seed);
        abstract protected DateTime GetFinishingBoundaryInstant(DateTime seed);
        abstract protected Int64 GetHumanReadableID(DateTime firstInstant, DateTime finishingBoundaryInstant);
        abstract protected Int64 GetSequentialFromEpochID(DateTime firstInstant, DateTime? epoch);
        
        protected Int64 GetCustomID(Func<DateTime, DateTime, Int64> strategy)
        {
            return strategy(FirstInstant, FinishingBoundaryInstant);
        }
    }
}
