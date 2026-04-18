using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DZMACLib.Tests
{
    [TestClass]
    public class CacheTests
    {
        [TestMethod]
        public void CacheShouldSupportRepeatedAddGetAndClear()
        {
            var dbPath = Path.Combine(Path.GetTempPath(), $"dzmac-cache-test-{Guid.NewGuid():N}.db");

            try
            {
                using (var cache = new Cache(dbPath))
                {
                    for (var i = 0; i < 25; i++)
                    {
                        cache.Clear();

                        cache.AddRange(new List<Vendor>
                    {
                        new Vendor("A1B2C3", "Vendor A"),
                        new Vendor("A1B2D4", "Vendor B")
                    });

                        var vendor = cache.Get("A1B2C3");
                        Assert.IsNotNull(vendor);
                        Assert.AreEqual("Vendor A", vendor?.VendorName);
                    }
                }
            }
            finally
            {
                if (File.Exists(dbPath))
                {
                    File.Delete(dbPath);
                }
            }
        }

        [TestMethod]
        public void ReplaceAllShouldReplaceExistingRecords()
        {
            var dbPath = Path.Combine(Path.GetTempPath(), $"dzmac-cache-replace-test-{Guid.NewGuid():N}.db");

            try
            {
                using (var cache = new Cache(dbPath))
                {
                    cache.AddRange(new List<Vendor>
                    {
                        new Vendor("A1B2C3", "Vendor A")
                    });

                    cache.ReplaceAll(new List<Vendor>
                    {
                        new Vendor("D4E5F6", "Vendor B")
                    });

                    Assert.IsNull(cache.Get("A1B2C3"));

                    var replacement = cache.Get("D4E5F6");
                    Assert.IsNotNull(replacement);
                    Assert.AreEqual("Vendor B", replacement?.VendorName);
                }
            }
            finally
            {
                if (File.Exists(dbPath))
                {
                    File.Delete(dbPath);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void IndexerShouldThrowWhenIndexEqualsCount()
        {
            var dbPath = Path.Combine(Path.GetTempPath(), $"dzmac-cache-index-test-{Guid.NewGuid():N}.db");

            try
            {
                using (var cache = new Cache(dbPath))
                {
                    cache.AddRange(new List<Vendor>
                    {
                        new Vendor("A1B2C3", "Vendor A")
                    });

                    _ = cache[cache.Count];
                }
            }
            finally
            {
                if (File.Exists(dbPath))
                {
                    File.Delete(dbPath);
                }
            }
        }
    }
}
