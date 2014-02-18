using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JakeBladt.TimeBlocks;

using Xunit;

namespace JakeBladt.TimeBlocks.Tests
{
    public class MonthBlockTests
    {
        [Fact]
        public void TestCreateMonth()
        {
            var seedDate = new DateTime(2013, 2, 7);
            var startDt = new DateTime(2013, 2, 1);
            var boundingDt = new DateTime(2013, 3, 1);

            var month = new MonthBlock(seedDate);
            Assert.Equal(month.SeedDate, seedDate);
            Assert.Equal(month.FirstInstant, startDt);
            Assert.Equal(month.FinishingBoundaryInstant, boundingDt);
        }
    }
}
