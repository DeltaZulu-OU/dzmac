using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MacChanger.Tests
{
    [TestClass]
    public class AdapterTests
    {
        private IEnumerable<NetworkAdapter> _adapters;

        [TestInitialize]
        public void Initialize() => _adapters = NetworkAdapterFactory.GetNetworkAdapters();

        [TestMethod]
        public void AdapterListShouldNotBeNull() => Assert.IsNotNull(_adapters);

        [TestMethod]
        public void AdapterListShouldHaveAtLeastOneAdapter() => Assert.IsTrue(_adapters.Any());
    }
}
