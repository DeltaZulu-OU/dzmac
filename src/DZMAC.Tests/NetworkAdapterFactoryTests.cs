using Dzmac.Core;

namespace Dzmac.Tests
{
    [TestClass]
    public class NetworkAdapterFactoryTests
    {
        [DataTestMethod]
        [DataRow(true, false, false, false, AdapterKind.Physical)]
        [DataRow(true, true, false, false, AdapterKind.VirtualOrLogical)]
        [DataRow(true, false, true, false, AdapterKind.VirtualOrLogical)]
        [DataRow(true, false, false, true, AdapterKind.VirtualOrLogical)]
        [DataRow(false, false, false, false, AdapterKind.Unknown)]
        [DataRow(null, null, null, null, AdapterKind.Unknown)]
        public void ResolveMsftKind_PrioritizesVirtualSignalsOverHardware(
            bool? hardwareInterface,
            bool? virtualInterface,
            bool? imFilter,
            bool? endPointInterface,
            AdapterKind expected)
        {
            var kind = NetworkAdapterFactory.ResolveMsftKind(hardwareInterface, virtualInterface, imFilter, endPointInterface);
            Assert.AreEqual(expected, kind);
        }

        [DataTestMethod]
        [DataRow(true, "ROOT\\NET\\0000", AdapterKind.Physical)]
        [DataRow(false, "PCI\\VEN_8086&DEV_15B8", AdapterKind.VirtualOrLogical)]
        [DataRow(null, "PCI\\VEN_8086&DEV_15B8", AdapterKind.Physical)]
        [DataRow(null, "ROOT\\NET\\0000", AdapterKind.VirtualOrLogical)]
        [DataRow(null, null, AdapterKind.VirtualOrLogical)]
        public void ResolveWin32Kind_UsesPhysicalAdapterFirst_ThenPrefixFallback(
            bool? physicalAdapter,
            string? pnpDeviceId,
            AdapterKind expected)
        {
            var kind = NetworkAdapterFactory.ResolveWin32Kind(physicalAdapter, pnpDeviceId);
            Assert.AreEqual(expected, kind);
        }

        [DataTestMethod]
        [DataRow(AdapterKind.Physical, true, true)]
        [DataRow(AdapterKind.Physical, false, true)]
        [DataRow(AdapterKind.VirtualOrLogical, true, false)]
        [DataRow(AdapterKind.VirtualOrLogical, false, false)]
        [DataRow(AdapterKind.Unknown, true, true)]
        [DataRow(AdapterKind.Unknown, false, false)]
        public void ShouldTreatAsPhysical_FallsBackToValidMacWhenKindUnknown(
            AdapterKind kind,
            bool hasValidMac,
            bool expected)
        {
            var isPhysical = NetworkAdapterFactory.ShouldTreatAsPhysical(kind, hasValidMac);
            Assert.AreEqual(expected, isPhysical);
        }
    }
}
