using System.Threading;
using System.Threading.Tasks;
using Dzmac.Gui.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dzmac.Tests
{
    [TestClass]
    public class AdapterAdminCommandExecutorTests
    {
        [TestMethod]
        public async Task ExecuteAsync_ReturnsSuccess_WhenCommandSucceeds()
        {
            var executor = new AdapterAdminCommandExecutor(new AdapterAdminPolicy(timeoutSeconds: 5, retryCount: 2));
            var command = new AdapterAdminCommand("test", "adapter", () => (true, "OK"));

            var result = await executor.ExecuteAsync(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(AdapterAdminResultCode.Success, result.Code);
        }

        [TestMethod]
        public async Task ExecuteAsync_RetriesAndEventuallySucceeds_WhenTransientFailureOccurs()
        {
            var attempts = 0;
            var executor = new AdapterAdminCommandExecutor(new AdapterAdminPolicy(timeoutSeconds: 5, retryCount: 3));
            var command = new AdapterAdminCommand("test", "adapter", () =>
            {
                attempts++;
                return attempts < 2 ? (false, "try again") : (true, "done");
            });

            var result = await executor.ExecuteAsync(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(2, attempts);
        }

        [TestMethod]
        public async Task ExecuteAsync_ReturnsTimeout_WhenCommandExceedsTimeout()
        {
            var executor = new AdapterAdminCommandExecutor(new AdapterAdminPolicy(timeoutSeconds: 1, retryCount: 1));
            var command = new AdapterAdminCommand("test", "adapter", () =>
            {
                Thread.Sleep(1500);
                return (true, "late");
            });

            var result = await executor.ExecuteAsync(command, CancellationToken.None);

            Assert.AreEqual(AdapterAdminResultCode.Timeout, result.Code);
        }
    }
}