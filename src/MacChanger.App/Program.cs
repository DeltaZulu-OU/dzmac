using System;

namespace MacChanger.Cli
{
    internal static class Program
    {
        private const int OuiLength = 6;

        public static void Main()
        {
            var manager = new VendorManager();
            var list = manager.GetVendorList();

            foreach (var adapter in AdapterFactory.GetAdapters())
            {
                Console.WriteLine(adapter.ToString());

                Console.WriteLine($"MAC: {adapter.MacAddress}");

                var vendor = "Unknown";
                if (list.TryGetValue(adapter.MacAddress.ToString().Substring(0, OuiLength), out var vendorNames))
                {
                    vendor = string.Join(", ", vendorNames);
                }
                Console.WriteLine($"Vendor: {vendor}");
            }
        }
    }
}
