using System;
using System.Linq;
using Shouldly;
using Xunit;

using Gallery.Entities.Elections;
using Gallery.Entities.ImageGallery;
using Gallery.Migration;

namespace Gallery.Migration.Tests
{
    public class EmptyMockElectionSet : IElectionSet
    {
        public bool Store(Election election)
        {
            return true;
        }
    }

    public class TestableElectionMigrationHelper : ElectionMigrationHelper
    {
        public TestableElectionMigrationHelper() : base(new FileSystemImageGallery(@"C:\"), new EmptyMockElectionSet())
        {

        }

        [Fact]
        public void TestSpecialElectionTitleDoesntTruncate()
        {
            var title = this.SpecialElectionNameFromDirectoryName("2017Rookies");
            title.ShouldBe("2017 Rookies");
        }
    }
}
