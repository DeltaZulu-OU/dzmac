using Dzmac.Core;

namespace Dzmac.Tests
{
    [TestClass]
    public class NetworkAdapterFactoryTests
    {
        [DataTestMethod]
        [DataRow("ROOT\\NET\\0000", false)]
        [DataRow("PCI\\VEN_8086&DEV_15B8", true)]
        [DataRow(null, false)]
        public void ResolveWin32Kind_UsesPhysicalAdapterFirst_ThenPrefixFallback(
            string? pnpDeviceId,
            bool expected)
        {
            var isPhysical = NetworkAdapterFactory.IsLikelyPhysicalAdapter(pnpDeviceId);
            Assert.AreEqual(expected, isPhysical);
        }
    }
}
