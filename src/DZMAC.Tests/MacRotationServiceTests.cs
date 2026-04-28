using System.Management;
using System.Net.NetworkInformation;
using Dzmac.Core;

namespace Dzmac.Tests
{
    [TestClass]
    public class MacRotationServiceTests
    {
        private NetworkInterface? _loopback;

        [TestInitialize]
        public void Initialize() => _loopback = Array.Find(
                NetworkInterface.GetAllNetworkInterfaces(),
                n => n.NetworkInterfaceType == NetworkInterfaceType.Loopback);

        [TestMethod]
        public void TryRotateMac_ReturnsRegDenied_WhenEnsureNetworkAddressThrowsUnauthorizedAccess()
        {
            if (_loopback is null)
            {
                Assert.Inconclusive("No loopback interface available.");
            }

            var wmi = new FakeWmiClient(resolves: false);
            var registry = new FakeRegistryClient(throwOnEnsure: new UnauthorizedAccessException("access denied"));
            var adapter = new NetworkAdapter(_loopback, null, false, wmi, registry);
            var progress = new NoOpProgress();

            var (Success, Message) = MacRotationService.TryRotateMac(adapter, new MacAddress("020000000001"), true, progress);

            Assert.IsFalse(Success);
            Assert.IsTrue(Message.Contains("ERR_REG_DENIED"), $"Expected ERR_REG_DENIED in: {Message}");
        }

        [TestMethod]
        public void TryRotateMac_ReturnsRegDenied_WhenEnsureNetworkAddressThrowsSecurityException()
        {
            if (_loopback is null)
            {
                Assert.Inconclusive("No loopback interface available.");
            }

            var wmi = new FakeWmiClient(resolves: false);
            var registry = new FakeRegistryClient(throwOnEnsure: new System.Security.SecurityException("denied"));
            var adapter = new NetworkAdapter(_loopback, null, false, wmi, registry);
            var progress = new NoOpProgress();

            var (Success, Message) = MacRotationService.TryRotateMac(adapter, new MacAddress("020000000001"), true, progress);

            Assert.IsFalse(Success);
            Assert.IsTrue(Message.Contains("ERR_REG_DENIED"), $"Expected ERR_REG_DENIED in: {Message}");
        }

        [TestMethod]
        public void TryRotateMac_ReturnsWmiFail_WhenAdapterWmiUnavailable()
        {
            if (_loopback is null)
            {
                Assert.Inconclusive("No loopback interface available.");
            }

            var wmi = new FakeWmiClient(resolves: false);
            var registry = new FakeRegistryClient(throwOnEnsure: null);
            var adapter = new NetworkAdapter(_loopback, null, false, wmi, registry);
            var progress = new NoOpProgress();

            var (Success, Message) = MacRotationService.TryRotateMac(adapter, new MacAddress("020000000001"), true, progress);

            Assert.IsFalse(Success);
            Assert.IsTrue(Message.Contains("ERR_WMI_FAIL"), $"Expected ERR_WMI_FAIL in: {Message}");
        }

        [TestMethod]
        public void TryRotateMac_ThrowsArgumentNull_WhenProgressIsNull()
        {
            if (_loopback is null)
            {
                Assert.Inconclusive("No loopback interface available.");
            }

            var wmi = new FakeWmiClient(resolves: false);
            var registry = new FakeRegistryClient(throwOnEnsure: null);
            var adapter = new NetworkAdapter(_loopback, null, false, wmi, registry);

            Assert.ThrowsException<ArgumentNullException>(() =>
                MacRotationService.TryRotateMac(adapter, new MacAddress("020000000001"), true, progress: null));
        }

        // ── Fake collaborators ───────────────────────────────────────────────────

        private sealed class FakeWmiClient : AdapterWmiClient
        {
            private readonly bool _resolves;

            public FakeWmiClient(bool resolves)
            {
                _resolves = resolves;
            }

            public override bool TryResolveByConfigId(string configId, out ManagementObject? adapter, out ManagementObject? adapterConfig)
            {
                adapter = null;
                adapterConfig = null;
                return _resolves;
            }
        }

        private sealed class FakeRegistryClient : AdapterRegistryClient
        {
            private readonly Exception? _throwOnEnsure;

            public FakeRegistryClient(Exception? throwOnEnsure)
            {
                _throwOnEnsure = throwOnEnsure;
            }

            public override string TryResolveRegistryKey(string registryClassKey, string configId)
                => @"SYSTEM\FakeClass\0000";

            public override object? ReadValue(string registryKey, string valueName) => null;

            public override bool TryValidateAdapterDescription(string registryKey, string description) => true;

            public override void SetStringValue(string registryKey, string valueName, string value)
            { }

            public override void DeleteValue(string registryKey, string valueName)
            { }

            public override void DeleteKeyTree(string registryKey)
            { }

            public override void EnsureNetworkAddressParameter(string registryKey)
            {
                if (_throwOnEnsure is not null)
                {
                    throw _throwOnEnsure;
                }
            }
        }

        private sealed class NoOpProgress : IProgress<string>
        {
            public void Report(string value)
            { }
        }
    }
}