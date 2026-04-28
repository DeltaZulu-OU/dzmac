using System.Reflection;
using Dzmac.Cli;

namespace Dzmac.Tests
{
    [TestClass]
    public class CliHandlerTests
    {
        private static readonly MethodInfo TryParseMethod = typeof(CliHandler).GetMethod(
            "TryParse",
            BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException("CliHandler.TryParse not found.");

        [TestMethod]
        public void TryParseShouldRejectMissingConnectionNameValueWhenNextTokenIsAnotherOption()
        {
            var args = new object?[] { new[] { "-n", "-m" }, null, null };

            var parsed = (bool)TryParseMethod.Invoke(null, args)!;

            Assert.IsFalse(parsed);
            Assert.AreEqual("Missing value for option '-n'.", (string)args[2]!);
        }

        [TestMethod]
        public void TryParseShouldRejectMissingIpv4ValueWhenNextTokenIsAnotherOption()
        {
            var args = new object?[] { new string?[] { "-i", "-g", "10.0.0.1/1" }, null, null };

            var parsed = (bool)TryParseMethod.Invoke(null, args)!;

            Assert.IsFalse(parsed);
            Assert.AreEqual("Missing value for option '-i'.", (string)args[2]!);
        }
    }
}
