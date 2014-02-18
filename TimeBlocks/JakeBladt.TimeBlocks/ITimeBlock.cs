using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JakeBladt.TimeBlocks
{
    public interface ITimeBlock
    {
        DateTime SeedDate { get; }
        Int64 ID { get; }
    }
}
