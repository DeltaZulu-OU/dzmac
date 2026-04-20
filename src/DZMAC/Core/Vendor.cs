using System;

namespace Dzmac.Core
{
    /// <summary>
    ///     A DTO to carry an OUI-Vendor name pair
    /// </summary>
    internal readonly struct Vendor : IEquatable<Vendor>
    {
        public string Oui { get; }

        public string VendorName { get; }

        public Vendor(string oui, string vendorName)
        {
            Oui = oui;
            VendorName = vendorName;
        }

        public readonly bool Equals(Vendor other) => Oui == other.Oui && VendorName == other.VendorName;

        public override readonly string ToString() => $"[{Oui}] {VendorName}";
    }
}
