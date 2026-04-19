namespace Dzmac.Cli
{
    public static class CommandLineHelpContent
    {
        private const string HelpText =
@"DZMAC Command Line

Visit https://github.com/zbalkan/DZMAC/ for more information and usage examples.

Usage: DZMAC -n network_connection_name [options]

OPTIONS:

    MAC Address Options [-m/nm/r/nr/r02/nr02]:
        -m [mac_address]
            Sets persistent MAC address to selected network connection.
            mac_address format: 00:01:02:03:04:05
            For restoring to original MAC address, leave the field blank.

        -nm [mac_address]
            Sets MAC address using the same implementation as -m.
            mac_address format: 00:01:02:03:04:05
            For restoring to original MAC address, leave the field blank.

        -r
            Sets random MAC address selected from vendor list
            to selected network connection.

        -nr
            Sets random MAC address selected from vendor list
            to selected network connection.

        -r02
            Sets random MAC address with 0x02 as first octet,
            selected from vendor list, to selected network connection.

        -nr02
            Sets random MAC address with 0x02 as first octet,
            selected from vendor list, to selected network connection.

    IPv4 Options:
        -i ip_address_1/subnet_mask_1[,ip_address_2/subnet_mask_2,...]
            Sets IPv4 address to selected network connection.

        -g gateway_1/metric_1[,gateway_2/metric_2,...]
            Sets default gateway route for selected network connection.

        -d dns_server_1[,dns_server_2,...]
            Sets DNS Server (IPv4) address.

        -h
            Enabled DHCPv4 on selected network connection.

        -rl
            Releases IPv4 address allocated by DHCPv4 server.

        -rn
            Renews IPv4 address lease from DHCPv4 server.

    Other Options:
        -s
            Silent mode flag accepted by CLI.

        -re
            Resets network connection to apply changes.

        -di
            Disables network connection.

        -en
            Enables network connection to apply changes.

        -ro
            Restores the original MAC address record.

    Help Option:
        -help
            Displays this help text for reference.";

        public static string Text => HelpText;
    }
}