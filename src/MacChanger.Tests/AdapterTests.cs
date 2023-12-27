using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MacChanger.Tests
{
    [TestClass]
    public class AdapterTests
    {
        private IEnumerable<Adapter> _adapters;

        [TestInitialize]
        public void Initialize() => _adapters = AdapterFactory.GetAdapters();

        [TestMethod]
        public void AdapterListShouldNotBeNull() => Assert.IsNotNull(_adapters);

        [TestMethod]
        public void AdapterListShouldHaveAtLeastOneAdapter() => Assert.IsTrue(_adapters.Any());
    }
}
