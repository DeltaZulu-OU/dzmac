using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace MacChanger
{
    internal static class Downloader
    {
        private const string _ouiAddress = "https://standards-oui.ieee.org/oui/oui.txt";

        public static List<Vendor> GetAll() => Parse(Download());

        private static string Download()
        {
            Debug.WriteLine("Starting OUI list download from IEEE...");
            string oui;
            using (var client = new WebClient())
            {
                var ouiBytes = client.DownloadData(_ouiAddress);
                oui = System.Text.Encoding.UTF8.GetString(ouiBytes);
            }
            Debug.WriteLine("Completed OUI list download from IEEE.");

            return oui;
        }

        private static List<Vendor> Parse(string oui)
        {
            Debug.WriteLine("Parsing downloaded data...");
            var vendors = new List<Vendor>();
            const RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant;
            var pattern = new Regex(@"^(\w{6})\s+\(base 16\)\t+(.+)$", options);
            foreach (Match item in pattern.Matches(oui))
            {
                vendors.Add(new Vendor(item.Groups[1].Value, item.Groups[2].Value));
            }
            Debug.WriteLine("Parsing completed.");

            return vendors;
        }
    }
}