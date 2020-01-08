using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIF.SWE1.Interfaces;
using ToLowerPlugin;

namespace ToLowerTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ReplacePlusTest()
        {
            // Arrange
            string expected = "Ich bin ein Test.";
            string actual = "Ich+bin+ein+Test.";
            // Act
            actual = Class1.replacePlus(actual);
            // Assert
            Assert.AreEqual(expected, actual, "String plus didn't get replaced");
        }

        [TestMethod]
        public void UrlDecodeTest()
        {
            // Arrange
            string expected = "!ß#+";
            string actual = "!%C3%9F%23%2B";
            // Act
            actual = Class1.UrlDecode(actual);
            // Assert
            Assert.AreEqual(expected, actual, "Url didn't get decoded");
        }
    }
}
