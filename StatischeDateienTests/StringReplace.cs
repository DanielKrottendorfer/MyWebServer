using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIF.SWE1.Interfaces;
using StatischeDateien;


namespace StatischeDateienTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ReplaceSymbolsTest()
        {
            // Arrange
            string expected = "&amp&lt&gt<br>";
            string actual = "&<>\n";
            // Act
            actual = Class1.ReplaceSymbols(actual);
            // Assert
            Assert.AreEqual(expected, actual, "String didn't get replaced");
        }
    }
}
