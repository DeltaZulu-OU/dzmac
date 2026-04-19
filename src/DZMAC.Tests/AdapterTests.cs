using System.Collections.Generic;
using System.Linq;
using Dzmac.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dzmac.Tests
{
    [TestClass]
    public class AdapterTests
    {
        private IEnumerable<NetworkAdapter> _adapters;

        [TestInitialize]
        public void Initialize() => _adapters = NetworkAdapterFactory.GetNetworkAdapters();

        [TestMethod]
        public void AdapterListShouldNotBeNull() => Assert.IsNotNull(_adapters);

        [TestMethod]
        public void AdapterListEnumerationShouldBeStable() => Assert.IsNotNull(_adapters.ToList());

        [DataTestMethod]
        [DataRow("PCI\\VEN_8086&DEV_15B8", true)]
        [DataRow("USB\\VID_0BDA&PID_8153", true)]
        [DataRow("ACPI\\PNP0C14", true)]
        [DataRow("ROOT\\NET\\0000", false)]
        [DataRow("", false)]
        [DataRow(null, false)]
        public void IsLikelyPhysicalAdapter_UsesPnpBusPrefixes(string pnpDeviceId, bool expected)
            => Assert.AreEqual(expected, NetworkAdapterFactory.IsLikelyPhysicalAdapter(pnpDeviceId));
    }
}