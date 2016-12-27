using System;
using System.Linq;
using Shouldly;
using Xunit;

using Gallery.Migration;

namespace Gallery.Migration.Tests
{
    public class TestableElectionMigrationHelper : ElectionMigrationHelper
    {
        

        [Fact]
        public void TestSpecialElectionTitleDoesntTruncate()
        {
           
        }
    }
}
