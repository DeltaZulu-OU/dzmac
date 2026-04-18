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
    }
}
