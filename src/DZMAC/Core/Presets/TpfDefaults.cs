using System.Collections.Generic;

namespace Dzmac.Core.Presets
{
    internal static class TpfDefaults
    {
        public static TpfFile CreateDefaultFile()
        {
            var file = new TpfFile
            {
                SelectedPresetIndex = 1
            };

            file.Presets.AddRange(CreateDefaultPresets());
            return file;
        }

        public static List<TpfPreset> CreateDefaultPresets()
        {
            return new List<TpfPreset>
            {
                new TpfPreset
                {
                    Name = "Random MAC Address",
                    MacMode = TpfMacMode.Random
                },
                new TpfPreset
                {
                    Name = "Original MAC Address",
                    MacMode = TpfMacMode.Original
                },
                new TpfPreset
                {
                    Name = "Sample Network",
                    MacMode = TpfMacMode.Random,
                    Ipv4 = new TpfIpv4Settings
                    {
                        Enabled = true,
                        IsStatic = true,
                        Address = "192.168.1.2",
                        SubnetMask = "255.255.255.0",
                        GatewayEnabled = true,
                        DefaultGateway = "192.168.1.1",
                        GatewayMetric = 0,
                        DnsEnabled = true,
                        PrimaryDnsServer = "192.168.1.1"
                    }
                }
            };
        }
    }
}
