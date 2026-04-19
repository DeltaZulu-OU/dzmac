using System;

namespace Dzmac.Core
{
    /// <summary>
    ///     A DTO to carry an OUI-Vendor name pair
    /// </summary>
    public struct Vendor : IEquatable<Vendor>
    {
        public string Oui { get; set; }

        public string VendorName { get; set; }

        public Vendor(string oui, string vendorName) : this()
        {
            Oui = oui;
            VendorName = vendorName;
        }

        public readonly bool Equals(Vendor other) => Oui == other.Oui && VendorName == other.VendorName;

        public override readonly string ToString() => $"[{Oui}] {VendorName}";
    }
}