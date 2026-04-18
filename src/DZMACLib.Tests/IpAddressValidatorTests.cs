using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DZMACLib.Tests
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
    }
}
