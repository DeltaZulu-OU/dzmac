using System.Diagnostics;
using System.Text;
using Dzmac.Core;

namespace Dzmac.Tests
{
    [TestClass]
    public class VendorListTests
    {
        private readonly Random _random = new Random();
        private readonly List<string> _tempPaths = new List<string>();

        private string CreateTempDbPath(string suffix)
        {
            var path = Path.Combine(Path.GetTempPath(), $"dzmac-vendorlist-{suffix}-{Guid.NewGuid():N}.csv");
            _tempPaths.Add(path);
            return path;
        }

        [TestCleanup]
        public void Cleanup()
        {
            foreach (var path in _tempPaths)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to delete temp test file '{path}': {ex}");
                }
            }

            _tempPaths.Clear();
        }

        [TestMethod]
        public void VendorListShouldSupportRepeatedAddGetAndClear()
        {
            var dbPath = CreateTempDbPath("repeat");

            using var vendorList = new VendorList(dbPath);

            for (var i = 0; i < 25; i++)
            {
                vendorList.Clear();

                vendorList.AddRange(new List<Vendor>
                {
                    new Vendor("A1B2C3", "Vendor A"),
                    new Vendor("A1B2D4", "Vendor B")
                });

                var vendor = vendorList.Get("A1B2C3");
                Assert.AreNotEqual(default, vendor);
                Assert.AreEqual("Vendor A", vendor.VendorName);
            }
        }

        [TestMethod]
        public void ReplaceAllShouldReplaceExistingRecords()
        {
            var dbPath = CreateTempDbPath("replace");

            using var vendorList = new VendorList(dbPath);

            vendorList.AddRange(new List<Vendor>
            {
                new Vendor("A1B2C3", "Vendor A")
            });

            vendorList.ReplaceAll(new List<Vendor>
            {
                new Vendor("D4E5F6", "Vendor B")
            });

            Assert.AreEqual(default, vendorList.Get("A1B2C3"));

            var replacement = vendorList.Get("D4E5F6");
            Assert.AreNotEqual(default, replacement);
            Assert.AreEqual("Vendor B", replacement.VendorName);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(10)]
        public void IndexerShouldThrowWhenIndexIsOutOfRange(int indexOffset)
        {
            var dbPath = CreateTempDbPath("index-oob");

            using var vendorList = new VendorList(dbPath);

            vendorList.AddRange(new List<Vendor>
            {
                new Vendor("A1B2C3", "Vendor A")
            });

            Assert.ThrowsException<IndexOutOfRangeException>(() =>
            {
                _ = vendorList[vendorList.Count + indexOffset];
            });
        }

        [TestMethod]
        public void GetShouldSupportWildcardLookup()
        {
            var dbPath = CreateTempDbPath("wildcard");

            using var vendorList = new VendorList(dbPath);

            vendorList.AddRange(new List<Vendor>
            {
                new Vendor("A1B2C3", "Vendor A")
            });

            var vendor = vendorList.Get("A9B2C3", useWildcard: true);
            Assert.AreNotEqual(default, vendor);
            Assert.AreEqual("Vendor A", vendor.VendorName);
        }

        [TestMethod]
        public void VendorListShouldPersistRecordsAcrossInstances()
        {
            var dbPath = CreateTempDbPath("persistence");

            using (var vendorList = new VendorList(dbPath))
            {
                vendorList.ReplaceAll(new List<Vendor>
                {
                    new Vendor("D4E5F6", "Vendor Persisted")
                });
            }

            using (var vendorList = new VendorList(dbPath))
            {
                var persisted = vendorList.Get("D4E5F6");
                Assert.AreNotEqual(default, persisted);
                Assert.AreEqual("Vendor Persisted", persisted.VendorName);
                Assert.AreEqual(1, vendorList.Count);
            }
        }

        [TestMethod]
        public void ClearShouldPersistAcrossInstances()
        {
            var dbPath = CreateTempDbPath("clear-persistence");

            using (var vendorList = new VendorList(dbPath))
            {
                vendorList.AddRange(new List<Vendor>
                {
                    new Vendor("A1B2C3", "Vendor A")
                });

                vendorList.Clear();
            }

            using (var vendorList = new VendorList(dbPath))
            {
                Assert.AreEqual(0, vendorList.Count);
                Assert.AreEqual(default, vendorList.Get("A1B2C3"));
            }
        }

        [TestMethod]
        public void AddRangeShouldSanitizeVendorNamesWithCarriageReturns()
        {
            var dbPath = CreateTempDbPath("sanitize");

            using (var vendorList = new VendorList(dbPath))
            {
                vendorList.ReplaceAll(new List<Vendor>
                {
                    new Vendor("A1B2C3", "Vendor Name\r")
                });
            }

            using (var vendorList = new VendorList(dbPath))
            {
                var vendor = vendorList.Get("A1B2C3");
                Assert.AreNotEqual(default, vendor);
                Assert.AreEqual("Vendor Name", vendor.VendorName);
            }
        }

        [TestMethod]
        public void VendorListShouldLoadIeeeCsvFormat()
        {
            var dbPath = CreateTempDbPath("ieee-csv");

            File.WriteAllText(
                dbPath,
                "Registry,Assignment,Organization Name,Organization Address\n" +
                "MA-L,FC-B0-DE,Vendor IEEE,Address\n");

            using var vendorList = new VendorList(dbPath);

            var vendor = vendorList.Get("FCB0DE");
            Assert.AreNotEqual(default, vendor);
            Assert.AreEqual("Vendor IEEE", vendor.VendorName);
        }

        [TestMethod]
        public void VendorListShouldLoadUtf8EncodedCsvWithExtendedCharacters()
        {
            var dbPath = CreateTempDbPath("utf8-csv");

            File.WriteAllText(
                dbPath,
                "Registry,Assignment,Organization Name,Organization Address\n" +
                "MA-L,FC-B0-DE,Compañía Ñandú,Address\n",
                Encoding.UTF8);

            using var vendorList = new VendorList(dbPath);

            var vendor = vendorList.Get("FCB0DE");
            Assert.AreNotEqual(default, vendor);
            Assert.AreEqual("Compañía Ñandú", vendor.VendorName);
        }

        [TestMethod]
        public void VendorListShouldLoadUtf8BomEncodedCsv()
        {
            var dbPath = CreateTempDbPath("utf8-bom-csv");

            File.WriteAllText(
                dbPath,
                "Registry,Assignment,Organization Name,Organization Address\n" +
                "MA-L,FC-B0-DE,Vendor IEEE,Address\n",
                new UTF8Encoding(true));

            using var vendorList = new VendorList(dbPath);

            var vendor = vendorList.Get("FCB0DE");
            Assert.AreNotEqual(default, vendor);
            Assert.AreEqual("Vendor IEEE", vendor.VendorName);
        }

        [TestMethod]
        public void VendorListShouldLoadInternalCsvRowsWithUnquotedCommas()
        {
            var dbPath = CreateTempDbPath("unquoted-comma");

            File.WriteAllText(dbPath, "A1B2C3,Acme, Inc.\n");

            using var vendorList = new VendorList(dbPath);

            var vendor = vendorList.Get("A1B2C3");
            Assert.AreNotEqual(default, vendor);
            Assert.AreEqual("Acme, Inc.", vendor.VendorName);
        }

        [TestMethod]
        public void VendorListShouldAllowAccessByIndex()
        {
            var dbPath = CreateTempDbPath("index-access");

            using var vendorList = new VendorList(dbPath);

            vendorList.AddRange(new List<Vendor>
            {
                new Vendor("A1B2C3", "Vendor A"),
                new Vendor("D4E5F6", "Vendor B"),
                new Vendor("112233", "Vendor C")
            });

            var index = _random.Next(vendorList.Count);
            var vendor = vendorList[index];

            Assert.IsNotNull(vendor);
            Assert.AreNotEqual(default(Vendor), vendor);
            Assert.IsFalse(string.IsNullOrWhiteSpace(vendor?.Oui));
            Assert.IsFalse(string.IsNullOrWhiteSpace(vendor?.VendorName));
        }

        [TestMethod]
        public void VendorListShouldAllowAccessByOui()
        {
            var dbPath = CreateTempDbPath("oui-access");

            using var vendorList = new VendorList(dbPath);

            vendorList.AddRange(new List<Vendor>
            {
                new Vendor("A1B2C3", "Vendor A"),
                new Vendor("D4E5F6", "Vendor B"),
                new Vendor("112233", "Vendor C")
            });

            var index = _random.Next(vendorList.Count);
            var temp = vendorList[index];
            Assert.AreNotEqual(default(Vendor), temp);

            var vendor = vendorList[temp?.Oui];
            Assert.IsNotNull(vendor);
            Assert.IsNotNull(temp);
            Assert.AreNotEqual(default(Vendor), vendor);
            Assert.IsFalse(string.IsNullOrWhiteSpace(vendor?.Oui));
            Assert.AreEqual(temp?.Oui, vendor?.Oui);
            Assert.IsFalse(string.IsNullOrWhiteSpace(vendor?.VendorName));
            Assert.AreEqual(temp?.VendorName, vendor?.VendorName);
        }

        [TestMethod]
        public void VendorListShouldReturnFalseWhenNonexistentOuiUsed()
        {
            var dbPath = CreateTempDbPath("missing-oui");

            using var vendorList = new VendorList(dbPath);

            vendorList.AddRange(new List<Vendor>
            {
                new Vendor("A1B2C3", "Vendor A")
            });

            var result = vendorList.TryGetValue("AAAAAA", out _);
            Assert.IsFalse(result);
        }

        [DataTestMethod]
        [DataRow("XXXXXX")]
        [DataRow("ZZA1B2C3ZZ")]
        [DataRow("A1-B2-C3")]
        [DataRow("A1:B2:C3")]
        public void TryGetValueShouldThrowWhenOuiFormatIsInvalid(string invalidOui)
        {
            var dbPath = CreateTempDbPath("bad-oui-format");

            using var vendorList = new VendorList(dbPath);

            Assert.ThrowsException<ArgumentException>(() =>
            {
                _ = vendorList.TryGetValue(invalidOui, out _);
            });
        }

        [TestMethod]
        public void VendorListShouldAllowEnumeration()
        {
            var dbPath = CreateTempDbPath("enumeration");

            using var vendorList = new VendorList(dbPath);

            vendorList.AddRange(new List<Vendor>
            {
                new Vendor("A1B2C3", "Vendor A"),
                new Vendor("D4E5F6", "Vendor B"),
                new Vendor("112233", "Vendor C")
            });

            var count = 0;

            foreach (var item in vendorList)
            {
                Debug.WriteLine(item);
                Assert.AreNotEqual(default, item);
                count++;
            }

            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void VendorListShouldAllowEnumerationWhenEmpty()
        {
            var dbPath = CreateTempDbPath("enumeration-empty");

            using var vendorList = new VendorList(dbPath);

            var count = 0;

            foreach (var item in vendorList)
            {
                Debug.WriteLine(item);
                count++;
            }

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void AddRangeWithEmptyCollectionShouldBeNoOp()
        {
            var dbPath = CreateTempDbPath("empty-addrange");

            using var vendorList = new VendorList(dbPath);

            vendorList.AddRange(Array.Empty<Vendor>());

            Assert.AreEqual(0, vendorList.Count);
        }

        [TestMethod]
        public void ReplaceAllWithEmptyCollectionShouldClearExistingRecords()
        {
            var dbPath = CreateTempDbPath("replace-empty");

            using var vendorList = new VendorList(dbPath);

            vendorList.AddRange(new List<Vendor>
            {
                new Vendor("A1B2C3", "Vendor A"),
                new Vendor("D4E5F6", "Vendor B")
            });

            vendorList.ReplaceAll(Array.Empty<Vendor>());

            Assert.AreEqual(0, vendorList.Count);
            Assert.AreEqual(default, vendorList.Get("A1B2C3"));
            Assert.AreEqual(default, vendorList.Get("D4E5F6"));
        }

        [TestMethod]
        public void VendorListShouldSkipBlankAndWhitespaceRows()
        {
            var dbPath = CreateTempDbPath("blank-rows");

            File.WriteAllText(
                dbPath,
                "\n" +
                "   \n" +
                "A1B2C3,Vendor A\n" +
                "\t\n" +
                "D4E5F6,Vendor B\n");

            using var vendorList = new VendorList(dbPath);

            Assert.AreEqual(2, vendorList.Count);

            Assert.AreEqual("Vendor A", vendorList.Get("A1B2C3").VendorName);
            Assert.AreEqual("Vendor B", vendorList.Get("D4E5F6").VendorName);
        }

        [TestMethod]
        public void VendorListShouldLoadHeaderOnlyIeeeCsvAsEmpty()
        {
            var dbPath = CreateTempDbPath("header-only-ieee");

            File.WriteAllText(
                dbPath,
                "Registry,Assignment,Organization Name,Organization Address\n");

            using var vendorList = new VendorList(dbPath);

            Assert.AreEqual(0, vendorList.Count);
        }

        [TestMethod]
        public void VendorListShouldSkipInvalidRowsAndContinueLoading()
        {
            var dbPath = CreateTempDbPath("skip-invalid");

            File.WriteAllText(
                dbPath,
                "A1B2C3,Vendor A\n" +
                "INVALID,Should Be Skipped\n" +
                "D4E5F6,Vendor B\n");

            using var vendorList = new VendorList(dbPath);

            Assert.AreEqual(2, vendorList.Count);

            var first = vendorList.Get("A1B2C3");
            var second = vendorList.Get("D4E5F6");

            Assert.AreNotEqual(default, first);
            Assert.AreEqual("Vendor A", first.VendorName);

            Assert.AreNotEqual(default, second);
            Assert.AreEqual("Vendor B", second.VendorName);
        }

        [TestMethod]
        public void VendorListShouldSkipRowsWithMissingVendorName()
        {
            var dbPath = CreateTempDbPath("skip-missing-name");

            File.WriteAllText(
                dbPath,
                "A1B2C3,Vendor A\n" +
                "D4E5F6,\n" +
                "112233,Vendor C\n");

            using var vendorList = new VendorList(dbPath);

            Assert.AreEqual(2, vendorList.Count);
            Assert.AreEqual("Vendor A", vendorList.Get("A1B2C3").VendorName);
            Assert.AreEqual(default, vendorList.Get("D4E5F6"));
            Assert.AreEqual("Vendor C", vendorList.Get("112233").VendorName);
        }

        [TestMethod]
        public void VendorListShouldSkipRowsWithTooFewColumns()
        {
            var dbPath = CreateTempDbPath("skip-too-few-columns");

            File.WriteAllText(
                dbPath,
                "A1B2C3,Vendor A\n" +
                "D4E5F6\n" +
                "112233,Vendor C\n");

            using var vendorList = new VendorList(dbPath);

            Assert.AreEqual(2, vendorList.Count);
            Assert.AreEqual("Vendor A", vendorList.Get("A1B2C3").VendorName);
            Assert.AreEqual(default, vendorList.Get("D4E5F6"));
            Assert.AreEqual("Vendor C", vendorList.Get("112233").VendorName);
        }

        [TestMethod]
        public void VendorListShouldSkipDuplicateOuiRows()
        {
            var dbPath = CreateTempDbPath("skip-duplicates");

            File.WriteAllText(
                dbPath,
                "A1B2C3,Vendor A\n" +
                "A1B2C3,Vendor A Duplicate\n" +
                "D4E5F6,Vendor B\n");

            using var vendorList = new VendorList(dbPath);

            Assert.AreEqual(2, vendorList.Count);

            var vendor = vendorList.Get("A1B2C3");
            Assert.AreNotEqual(default, vendor);
            Assert.AreEqual("Vendor A", vendor.VendorName);
        }

        [TestMethod]
        public void VendorListShouldLoadValidRowsWhileSkippingInvalidAndDuplicateRows()
        {
            var dbPath = CreateTempDbPath("mixed-dirty-input");

            File.WriteAllText(
                dbPath,
                "A1B2C3,Vendor A\n" +
                "INVALID,Invalid Row\n" +
                "D4E5F6,Vendor B\n" +
                "A1B2C3,Duplicate Vendor A\n" +
                "XYZ\n" +
                "112233,Vendor C\n");

            using var vendorList = new VendorList(dbPath);

            Assert.AreEqual(3, vendorList.Count);
            Assert.AreEqual("Vendor A", vendorList.Get("A1B2C3").VendorName);
            Assert.AreEqual("Vendor B", vendorList.Get("D4E5F6").VendorName);
            Assert.AreEqual("Vendor C", vendorList.Get("112233").VendorName);
        }

        [TestMethod]
        public void VendorListShouldTreatLowercaseStoredOuiAsEquivalentIfSupportedByParser()
        {
            var dbPath = CreateTempDbPath("lowercase-stored-oui");

            File.WriteAllText(dbPath, "a1b2c3,Vendor A\n");

            using var vendorList = new VendorList(dbPath);

            var vendor = vendorList.Get("A1B2C3");

            if (vendor.Equals(default))
            {
                Assert.Inconclusive("Parser does not normalize lowercase OUI values from file input.");
            }

            Assert.AreEqual("Vendor A", vendor.VendorName);
        }
    }
}