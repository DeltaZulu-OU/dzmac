using Dzmac.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dzmac.Tests
{
    [TestClass]
    public class IpAddressValidatorTests
    {
        [TestMethod]
        public void TryValidateIpv4Address_RejectsIpv6()
        {
            var result = IpAddressValidator.TryValidateIpv4Address("2001:db8::1", out var _);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryValidateIpv4Address_AcceptsDottedDecimalAddress()
        {
            var result = IpAddressValidator.TryValidateIpv4Address("10.1.1.1", out var normalized);
            Assert.IsTrue(result);
            Assert.AreEqual("10.1.1.1", normalized);
        }

        [TestMethod]
        public void TryValidateIpv4Address_RejectsLeadingZeroOctets()
        {
            var result = IpAddressValidator.TryValidateIpv4Address("010.001.001.001", out var normalized);
            Assert.IsFalse(result);
            Assert.AreEqual(string.Empty, normalized);
        }

        [TestMethod]
        public void TryValidateIpv4SubnetMask_RejectsNonAddressInput()
        {
            var result = IpAddressValidator.TryValidateIpv4SubnetMask("not-an-ip", out var normalized);
            Assert.IsFalse(result);
            Assert.AreEqual(string.Empty, normalized);
        }

        [TestMethod]
        public void TryValidateIpv4SubnetMask_RejectsNonContiguousMask()
        {
            var result = IpAddressValidator.TryValidateIpv4SubnetMask("255.0.255.0", out var _);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryValidateIpv4SubnetMask_AcceptsValidMask()
        {
            var result = IpAddressValidator.TryValidateIpv4SubnetMask("255.255.255.0", out var normalized);
            Assert.IsTrue(result);
            Assert.AreEqual("255.255.255.0", normalized);
        }

        [DataTestMethod]
        [DataRow("0.0.0.0")]
        [DataRow("255.255.255.255")]
        public void TryValidateIpv4SubnetMask_AcceptsBoundaryMasks(string mask)
        {
            var result = IpAddressValidator.TryValidateIpv4SubnetMask(mask, out var normalized);
            Assert.IsTrue(result);
            Assert.AreEqual(mask, normalized);
        }
    }
}
