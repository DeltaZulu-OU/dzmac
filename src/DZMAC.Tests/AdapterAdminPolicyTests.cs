using System;
using Dzmac.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dzmac.Tests
{
    [TestClass]
    public class AdapterAdminPolicyTests
    {
        [TestMethod]
        public void Constructor_ShouldClampTimeoutAndRetryToAtLeastOne()
        {
            var policy = new AdapterAdminPolicy(timeoutSeconds: 0, retryCount: -10);

            Assert.AreEqual(1, policy.TimeoutSeconds);
            Assert.AreEqual(1, policy.RetryCount);
        }

        [TestMethod]
        public void Command_ShouldUseUnknownAdapterName_WhenWhitespaceAdapterNameProvided()
        {
            var command = new AdapterAdminCommand("test-command", "   ", () => (true, "ok"));

            Assert.AreEqual("unknown", command.AdapterName);
        }

        [TestMethod]
        public void Command_ShouldThrow_WhenNameIsWhitespace()
        {
            Assert.ThrowsException<ArgumentException>(() => new AdapterAdminCommand("  ", "adapter", () => (true, "ok")));
        }

        [TestMethod]
        public void FromConfig_ShouldThrow_WhenSettingsIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => AdapterAdminPolicy.FromConfig(null));
        }
    }
}
