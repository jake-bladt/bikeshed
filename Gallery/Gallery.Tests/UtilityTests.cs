using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Gallery.Utilities;

namespace Gallery.Tests
{
    [TestClass]
    public class UtilityTests
    {
        [TestMethod]
        public void TestSimpleName()
        {
            Assert.Equals("Susan", NameMapper.DirectoryNameToDisplayName("susan"));
        }
    }
}
