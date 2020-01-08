using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIF.SWE1.Interfaces;
using SomePlugin;

namespace TemperatureTests
{
    [TestClass]
    public class Temperature
    {
        [TestMethod]
        public void TemperatureStringTest()
        {
            // Arrange
            string expected = "34";
            string actual = "35";
            // Act
            actual = TemperaturePlugin.lowerString(actual);
            // Assert
            Assert.AreEqual(expected, actual, "String did't go lower");
        }

        [TestMethod]
        public void ArrayCheckerTest()
        {
            // Arrange
            bool expectedOne = true;
            bool expectedTwo = false;
            string[] testOne;
            string[] testTwo;
            bool actualOne, actualTwo;
            testOne = new string[5]{"First","Value","Third","Value","Last"};
            testTwo = new string[3] { "First", "", "Third" };
            // Act
            actualOne = TemperaturePlugin.arrayChecker(testOne);
            actualTwo = TemperaturePlugin.arrayChecker(testTwo);
            // Assert
            Assert.AreEqual(expectedOne, actualOne, "arrays strange");
            Assert.AreEqual(expectedTwo, actualTwo, "arrays strange");
        }
    }
}
