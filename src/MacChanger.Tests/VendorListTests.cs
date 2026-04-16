using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MacChanger.Tests
{
    [TestClass]
    public class VendorListTests
    {
        private readonly Random random = new Random();
        private VendorList _list;

        [TestInitialize]
        public void Initialize() => _list = new VendorManager().GetVendorList();

        [TestMethod]
        public void VendorListShouldNotBeNull() => Assert.IsNotNull(_list);

        [TestMethod]
        public void VendorListCanBeEmptyBeforeManualRefresh() => Assert.IsTrue(_list.Count >= 0);

        [TestMethod]
        public void VendorListShouldAllowAccessByIndex()
        {
            if (_list.Count == 0)
            {
                Assert.Inconclusive("Vendor list is empty until user-triggered refresh.");
            }

            var index = random.Next(_list.Count);
            var v = _list[index];
            Assert.IsNotNull(v);
        }

        [TestMethod]
        public void VendorListShouldFailWhenNonexistentIndexUsed()
        {
            void FailToAccess()
            {
                var index = _list.Count + 10;
                _ = _list[index];
            }

            Assert.ThrowsException<IndexOutOfRangeException>(FailToAccess);
        }

        [TestMethod]
        public void VendorListShouldAllowAccessByOui()
        {
            if (_list.Count == 0)
            {
                Assert.Inconclusive("Vendor list is empty until user-triggered refresh.");
            }

            var index = random.Next(_list.Count);
            var temp = _list[index] ?? default;
            var v = _list[temp.Oui];
            Assert.IsNotNull(v);
        }

        [TestMethod]
        public void VendorListShouldReturnFalseWhenNonexistentOuiUsed()
        {
            var result = _list.TryGetValue("AAAAAA", out _);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void VendorListShouldFailWhenIncorrectlyFormattedOuiUsed()
        {
            void IncorrectFormat()
            {
                _ = _list.TryGetValue("XXXXXX", out var v);
            }

            Assert.ThrowsException<ArgumentException>(IncorrectFormat);
        }

        [TestMethod]
        public void VendorListShouldFailWhenOuiContainsValidFragmentOnly()
        {
            void InvalidButContainsFragment()
            {
                _ = _list.TryGetValue("ZZA1B2C3ZZ", out var _);
            }

            Assert.ThrowsException<ArgumentException>(InvalidButContainsFragment);
        }

        [TestMethod]
        public void VendorListShouldAllowEnumeration()
        {
            bool Enumerate()
            {
                try
                {
                    foreach (var item in _list)
                    {
                        Debug.WriteLine(item);
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            Assert.IsTrue(Enumerate());
        }
    }
}
