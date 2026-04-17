using System;

namespace MacChanger.Cli
{
    internal static class Program
    {
        private const int OuiLength = 6;

        public static void Main()
        {
            Diagnostics.Info("application_start", ("host", "cli"));

            var manager = new VendorManager();
            var list = manager.GetVendorList();

            foreach (var adapter in NetworkAdapterFactory.GetNetworkAdapters())
            {
                Console.WriteLine(adapter.ToString());

                Console.WriteLine($"MAC: {adapter.OriginalMacAddress}");

                var vendor = "Unknown";
                if (list.TryGetValue(adapter.OriginalMacAddress.ToString().Substring(0, OuiLength), out var vendorNames))
                {
                    vendor = string.Join(", ", vendorNames);
                }
                Console.WriteLine($"Vendor: {vendor}");
            }

            Diagnostics.Info("application_stop", ("host", "cli"));
        }
    }
}
