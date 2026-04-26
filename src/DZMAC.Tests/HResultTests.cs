using Dzmac.Core;

namespace Dzmac.Tests
{
    [TestClass]
    public class HResultTests
    {
        [DataTestMethod]
        [DataRow(0, "Successful completion, no reboot required")]
        [DataRow(1, "Successful completion, reboot required")]
        [DataRow(91, "Access denied")]
        [DataRow(101, "Other")]
        public void TranslateErrorCode_ReturnsKnownMessages(int code, string expected)
        {
            var translated = HResult.TranslateErrorCode(code);

            Assert.AreEqual(expected, translated);
        }

        [TestMethod]
        public void TranslateErrorCode_ReturnsEmptyString_ForUnknownCode()
        {
            var translated = HResult.TranslateErrorCode(9999);

            Assert.AreEqual(string.Empty, translated);
        }
    }
}