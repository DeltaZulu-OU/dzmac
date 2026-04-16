using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MacChanger.Tests
{
    [TestClass]
    public class MacAddressTests
    {
        [TestMethod]
        public void IsValidMacShouldAcceptTwelveUppercaseHexCharacters()
        {
            Assert.IsTrue(MacAddress.IsValidMac("A1B2C3D4E5F6"));
        }

        [TestMethod]
        public void IsValidMacShouldRejectInvalidCharacters()
        {
            Assert.IsFalse(MacAddress.IsValidMac("A1B2C3D4E5FG"));
        }

        [TestMethod]
        public void IsValidMacShouldRejectWrongLength()
        {
            Assert.IsFalse(MacAddress.IsValidMac("A1B2C3D4E5"));
        }
    }
}
