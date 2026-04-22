using Dzmac.Core.Presets;
using System.IO;

namespace DZMAC.Tests;

[TestClass]
public class TpfSerializerTests
{
    [TestMethod]
    public void SaveAndLoad_RoundTripsSupportedSubset()
    {
        var file = new TpfFile
        {
            Version = 1,
            Reserved = 0,
            SelectedPresetIndex = 1
        };

        file.Presets.Add(new TpfPreset
        {
            Name = "Random MAC",
            MacMode = TpfMacMode.Random
        });

        file.Presets.Add(new TpfPreset
        {
            Name = "Static IPv4",
            MacMode = TpfMacMode.Custom,
            CustomMac = "00-11-22-33-44-55",
            Ipv4 = new TpfIpv4Settings
            {
                Enabled = true,
                IsStatic = true,
                Address = "192.168.1.10",
                SubnetMask = "255.255.255.0",
                GatewayEnabled = true,
                DefaultGateway = "192.168.1.1",
                GatewayMetric = 5,
                DnsEnabled = true,
                PrimaryDnsServer = "1.1.1.1"
            }
        });

        var bytes = TpfSerializer.Save(file);
        var loaded = TpfSerializer.Load(bytes);

        Assert.AreEqual(2, loaded.Presets.Count);
        Assert.AreEqual("Random MAC", loaded.Presets[0].Name);
        Assert.AreEqual(TpfMacMode.Custom, loaded.Presets[1].MacMode);
        Assert.AreEqual("00-11-22-33-44-55", loaded.Presets[1].CustomMac);
        Assert.IsNotNull(loaded.Presets[1].Ipv4);
        Assert.AreEqual("192.168.1.10", loaded.Presets[1].Ipv4!.Address);
        Assert.AreEqual("1.1.1.1", loaded.Presets[1].Ipv4.PrimaryDnsServer);
    }

    [TestMethod]
    public void Load_RecoversFromResidualBytesBetweenPresets()
    {
        var file = new TpfFile();
        file.Presets.Add(new TpfPreset { Name = "Random MAC Address", MacMode = TpfMacMode.Random });
        file.Presets.Add(new TpfPreset { Name = "Original MAC Address", MacMode = TpfMacMode.Original });
        file.Presets.Add(new TpfPreset
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
        });

        var bytes = TpfSerializer.Save(file);
        var name2 = System.Text.Encoding.Unicode.GetBytes("Original MAC Address");
        var index = FindUtf16Sequence(bytes, name2);
        Assert.IsTrue(index > 0, "Failed to locate second preset name in payload.");

        var withResidual = new byte[bytes.Length + 6];
        Array.Copy(bytes, 0, withResidual, 0, index - 4);
        withResidual[index - 4] = 0xFF;
        withResidual[index - 3] = 0xFF;
        withResidual[index - 2] = 0xFF;
        withResidual[index - 1] = 0xFF;
        withResidual[index] = 0x00;
        withResidual[index + 1] = 0x00;
        Array.Copy(bytes, index - 4, withResidual, index + 2, bytes.Length - (index - 4));

        var loaded = TpfSerializer.Load(withResidual);

        Assert.AreEqual(3, loaded.Presets.Count);
        Assert.AreEqual("Random MAC Address", loaded.Presets[0].Name);
        Assert.AreEqual("Original MAC Address", loaded.Presets[1].Name);
        Assert.AreEqual("Sample Network", loaded.Presets[2].Name);
    }


    [TestMethod]
    public void CreateDefaultFile_ContainsThreeExpectedPresets()
    {
        var file = TpfDefaults.CreateDefaultFile();

        Assert.AreEqual(3, file.Presets.Count);
        Assert.AreEqual("Random MAC Address", file.Presets[0].Name);
        Assert.AreEqual(TpfMacMode.Random, file.Presets[0].MacMode);
        Assert.AreEqual("Original MAC Address", file.Presets[1].Name);
        Assert.AreEqual(TpfMacMode.Original, file.Presets[1].MacMode);
        Assert.AreEqual("Sample Network", file.Presets[2].Name);
        Assert.AreEqual(TpfMacMode.Random, file.Presets[2].MacMode);
        Assert.AreEqual((byte)1, file.SelectedPresetIndex);
    }

    [TestMethod]
    public void CreateDefaultFile_SampleNetworkContainsStaticIpv4Settings()
    {
        var sample = TpfDefaults.CreateDefaultFile().Presets[2];

        Assert.IsNotNull(sample.Ipv4);
        Assert.IsTrue(sample.Ipv4!.Enabled);
        Assert.IsTrue(sample.Ipv4.IsStatic);
        Assert.AreEqual("192.168.1.2", sample.Ipv4.Address);
        Assert.AreEqual("255.255.255.0", sample.Ipv4.SubnetMask);
        Assert.IsTrue(sample.Ipv4.GatewayEnabled);
        Assert.AreEqual("192.168.1.1", sample.Ipv4.DefaultGateway);
        Assert.AreEqual(0, sample.Ipv4.GatewayMetric);
        Assert.IsTrue(sample.Ipv4.DnsEnabled);
        Assert.AreEqual("192.168.1.1", sample.Ipv4.PrimaryDnsServer);
    }

    [TestMethod]
    public void Load_RejectsOversizedPayload()
    {
        var oversized = new byte[(1024 * 1024) + 1];
        oversized[0] = 1;
        oversized[1] = 0;
        oversized[2] = 0;
        oversized[3] = 0;

        Assert.ThrowsException<InvalidDataException>(() => TpfSerializer.Load(oversized));
    }

    [TestMethod]
    public void Load_RejectsPresetWithInvalidCustomMacPayload()
    {
        var file = new TpfFile();
        file.Presets.Add(new TpfPreset
        {
            Name = "Unsafe",
            MacMode = TpfMacMode.Custom,
            CustomMac = "00-11-22-33-44-55 && calc.exe"
        });

        var payload = TpfSerializer.Save(file);
        var loaded = TpfSerializer.Load(payload);

        Assert.AreEqual(0, loaded.Presets.Count);
        Assert.AreEqual(1, loaded.ParseWarnings.Count);
    }

    private static int FindUtf16Sequence(byte[] data, byte[] sequence)
    {
        for (var i = 0; i <= data.Length - sequence.Length; i++)
        {
            var found = true;
            for (var j = 0; j < sequence.Length; j++)
            {
                if (data[i + j] != sequence[j])
                {
                    found = false;
                    break;
                }
            }

            if (found)
            {
                return i;
            }
        }

        return -1;
    }
}
