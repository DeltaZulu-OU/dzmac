using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DZMACLib.Tests
{
    [TestClass]
    public class DownloaderTests
    {
        [TestMethod]
        public void TryExtractSha256ShouldReadHashFromManifestText()
        {
            const string expected = "2CF24DBA5FB0A30E26E83B2AC5B9E29E1B161E5C1FA7425E73043362938B9824";
            var manifest = $"{expected}  oui.txt";

            var result = Downloader.TryExtractSha256(manifest, out var hash);

            Assert.IsTrue(result);
            Assert.AreEqual(expected, hash);
        }

        [TestMethod]
        public void TryExtractSha256ShouldFailWhenManifestHasNoHash()
        {
            var result = Downloader.TryExtractSha256("not-a-hash", out var hash);

            Assert.IsFalse(result);
            Assert.AreEqual(string.Empty, hash);
        }

        [TestMethod]
        public void ComputeSha256ShouldReturnDeterministicValue()
        {
            var hash = Downloader.ComputeSha256("hello");

            Assert.AreEqual("2CF24DBA5FB0A30E26E83B2AC5B9E29E1B161E5C1FA7425E73043362938B9824", hash);
        }
    }
}
