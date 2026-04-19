using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Dzmac.Gui.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dzmac.Tests
{
    [TestClass]
    public class CacheTests
    {
        [TestMethod]
        public void CacheShouldSupportRepeatedAddGetAndClear()
        {
            var dbPath = Path.Combine(Path.GetTempPath(), $"dzmac-cache-test-{Guid.NewGuid():N}.csv");

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
                        Assert.AreEqual("Vendor A", vendor.VendorName);
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
            var dbPath = Path.Combine(Path.GetTempPath(), $"dzmac-cache-replace-test-{Guid.NewGuid():N}.csv");

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

                    Assert.AreEqual(cache.Get("A1B2C3"), default(Vendor));

                    var replacement = cache.Get("D4E5F6");
                    Assert.AreNotEqual(replacement, default(Vendor));
                    Assert.AreEqual("Vendor B", replacement.VendorName);
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
            var dbPath = Path.Combine(Path.GetTempPath(), $"dzmac-cache-index-test-{Guid.NewGuid():N}.csv");

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

        [TestMethod]
        public void GetShouldSupportWildcardLookup()
        {
            var dbPath = Path.Combine(Path.GetTempPath(), $"dzmac-cache-wildcard-test-{Guid.NewGuid():N}.csv");

            try
            {
                using (var cache = new Cache(dbPath))
                {
                    cache.AddRange(new List<Vendor>
                    {
                        new Vendor("A1B2C3", "Vendor A")
                    });

                    var vendor = cache.Get("A9B2C3", useWildcard: true);
                    Assert.IsNotNull(vendor);
                    Assert.AreEqual("Vendor A", vendor.VendorName);
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
        public void CacheShouldPersistRecordsAcrossInstances()
        {
            var dbPath = Path.Combine(Path.GetTempPath(), $"dzmac-cache-persistence-test-{Guid.NewGuid():N}.csv");

            try
            {
                using (var cache = new Cache(dbPath))
                {
                    cache.ReplaceAll(new List<Vendor>
                    {
                        new Vendor("D4E5F6", "Vendor Persisted")
                    });
                }

                using (var cache = new Cache(dbPath))
                {
                    var persisted = cache.Get("D4E5F6");
                    Assert.IsNotNull(persisted);
                    Assert.AreEqual("Vendor Persisted", persisted.VendorName);
                    Assert.AreEqual(1, cache.Count);
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
        public void AddRangeShouldSanitizeVendorNamesWithCarriageReturns()
        {
            var dbPath = Path.Combine(Path.GetTempPath(), $"dzmac-cache-sanitize-test-{Guid.NewGuid():N}.csv");

            try
            {
                using (var cache = new Cache(dbPath))
                {
                    cache.ReplaceAll(new List<Vendor>
                    {
                        new Vendor("A1B2C3", "Vendor Name\r")
                    });
                }

                using (var cache = new Cache(dbPath))
                {
                    var vendor = cache.Get("A1B2C3");
                    Assert.IsNotNull(vendor);
                    Assert.AreEqual("Vendor Name", vendor.VendorName);
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
        public void CacheShouldLoadIeeeCsvFormat()
        {
            var dbPath = Path.Combine(Path.GetTempPath(), $"dzmac-cache-ieee-csv-test-{Guid.NewGuid():N}.csv");

            try
            {
                File.WriteAllText(dbPath,
                    "Registry,Assignment,Organization Name,Organization Address\n" +
                    "MA-L,FC-B0-DE,Vendor IEEE,Address\n");

                using (var cache = new Cache(dbPath))
                {
                    var vendor = cache.Get("FCB0DE");
                    Assert.IsNotNull(vendor);
                    Assert.AreEqual("Vendor IEEE", vendor.VendorName);
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
        public void CacheShouldLoadAnsiEncodedCsvWithExtendedCharacters()
        {
            var dbPath = Path.Combine(Path.GetTempPath(), $"dzmac-cache-encoding-csv-test-{Guid.NewGuid():N}.csv");

            try
            {
                File.WriteAllText(
                    dbPath,
                    "Registry,Assignment,Organization Name,Organization Address\nMA-L,FC-B0-DE,Compañía Ñandú,Address\n",
                    Encoding.UTF8);

                using (var cache = new Cache(dbPath))
                {
                    var vendor = cache.Get("FCB0DE");
                    Assert.IsNotNull(vendor);
                    Assert.AreEqual("Compañía Ñandú", vendor.VendorName);
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
        public void CacheShouldLoadInternalCsvRowsWithUnquotedCommas()
        {
            var dbPath = Path.Combine(Path.GetTempPath(), $"dzmac-cache-unquoted-comma-test-{Guid.NewGuid():N}.csv");

            try
            {
                File.WriteAllText(dbPath, "A1B2C3,Acme, Inc.\n");

                using (var cache = new Cache(dbPath))
                {
                    var vendor = cache.Get("A1B2C3");
                    Assert.IsNotNull(vendor);
                    Assert.AreEqual("Acme, Inc.", vendor.VendorName);
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
