using System.Net.NetworkInformation;
using Dzmac.Core;

namespace Dzmac.Tests
{
    [TestClass]
    public class MacAddressTests
    {
        [TestMethod]
        public void IsValidMacShouldAcceptTwelveUppercaseHexCharacters() => Assert.IsTrue(MacAddress.IsValidMac("A1B2C3D4E5F6"));

        [TestMethod]
        public void IsValidMacShouldRejectInvalidCharacters() => Assert.IsFalse(MacAddress.IsValidMac("A1B2C3D4E5FG"));

        [TestMethod]
        public void IsValidMacShouldRejectWrongLength() => Assert.IsFalse(MacAddress.IsValidMac("A1B2C3D4E5"));

        [TestMethod]
        public void EqualsShouldReturnFalseForDifferentObjectType()
        {
            var mac = new MacAddress("A1B2C3D4E5F6");

            Assert.IsFalse(mac.Equals("A1B2C3D4E5F6"));
        }

        [TestMethod]
        public void ConstructorShouldThrowForInvalidPhysicalAddress()
        {
            var invalidPhysicalAddress = new PhysicalAddress(Array.Empty<byte>());

            _ = Assert.ThrowsException<ArgumentException>(() => new MacAddress(invalidPhysicalAddress));
        }

        [TestMethod]
        public void ConstructorShouldAcceptValidPhysicalAddress()
        {
            var physicalAddress = PhysicalAddress.Parse("AA-BB-CC-DD-EE-FF");

            var mac = new MacAddress(physicalAddress);

            Assert.AreEqual("AABBCCDDEEFF", mac.ToString());
        }

        [TestMethod]
        public void GetNewMacForOuiShouldThrowWhenOuiIsNull()
        {
            _ = Assert.ThrowsException<ArgumentNullException>(() => MacAddress.GetNewMac((string)null));
        }

        [TestMethod]
        public void GetNewMacForOuiShouldThrowWhenOuiHasInvalidFormat()
        {
            _ = Assert.ThrowsException<ArgumentException>(() => MacAddress.GetNewMac("ZZZZZZ"));
            _ = Assert.ThrowsException<ArgumentException>(() => MacAddress.GetNewMac("AABB0"));
            _ = Assert.ThrowsException<ArgumentException>(() => MacAddress.GetNewMac("AABBCCDD"));
        }

        [TestMethod]
        public void GetNewMacShouldSetLocallyAdministeredBitAndClearMulticastBit()
        {
            var mac = MacAddress.GetNewMac().ToString();
            var firstOctet = Convert.ToByte(mac.Substring(0, 2), 16);

            Assert.AreEqual(0x02, firstOctet & 0x02, "Expected locally administered bit to be set.");
            Assert.AreEqual(0x00, firstOctet & 0x01, "Expected multicast bit to be cleared.");
        }

        [TestMethod]
        public void AsLocallyAdministeredShouldReturnNewInstanceWithoutMutatingOriginal()
        {
            var original = new MacAddress("A1B2C3D4E5F6");

            var updated = original.AsLocallyAdministered();

            Assert.AreEqual("A1B2C3D4E5F6", original.ToString());
            Assert.AreEqual("A2B2C3D4E5F6", updated.ToString());
        }
    }
}