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
            Assert.AreEqual("Susan", NameMapper.DirectoryNameToDisplayName("susan"));
        }

        [TestMethod]
        public void TestLastInitial()
        {
            Assert.AreEqual("Suzie Q.", NameMapper.DirectoryNameToDisplayName("suzie.q"));
        }

        [TestMethod]
        public void TestFirstAndLast()
        {
            Assert.AreEqual("George Washington", NameMapper.DirectoryNameToDisplayName("george.washington"));
        }

        [TestMethod]
        public void TestHyphenated()
        {
            Assert.AreEqual("Mary-Anne", NameMapper.DirectoryNameToDisplayName("mary-anne"));
        }

        [TestMethod]
        public void TestHypenatedWithMiddleInitial()
        {
            Assert.AreEqual("Billy-Rae C.", NameMapper.DirectoryNameToDisplayName("billy-rae.c"));
        }

    }
}
