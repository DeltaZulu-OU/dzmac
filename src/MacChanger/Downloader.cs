#nullable enable

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MacChanger
{
    internal static class Downloader
    {
        private const string DefaultOuiAddress = "https://standards-oui.ieee.org/oui/oui.txt";
        private const int DefaultTimeoutSeconds = 15;
        private const int DefaultRetryCount = 3;

        public static List<Vendor> GetAll() => GetAllAsync(CancellationToken.None).GetAwaiter().GetResult();

        public static async Task<List<Vendor>> GetAllAsync(CancellationToken cancellationToken)
        {
            var responseText = await DownloadAsync(cancellationToken).ConfigureAwait(false);
            return Parse(responseText);
        }

        private static async Task<string> DownloadAsync(CancellationToken cancellationToken)
        {
            var ouiAddress = ConfigurationManager.AppSettings["MacChanger.OuiEndpoint"] ?? DefaultOuiAddress;
            var timeoutSeconds = ReadIntSetting("MacChanger.OuiDownloadTimeoutSeconds", DefaultTimeoutSeconds);
            var retryCount = Math.Max(1, ReadIntSetting("MacChanger.OuiDownloadRetryCount", DefaultRetryCount));

            using var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };

            for (var attempt = 1; attempt <= retryCount; attempt++)
            {
                try
                {
                    Diagnostics.Info("oui_download_attempt", ("attempt", attempt), ("endpoint", ouiAddress));
                    using var request = new HttpRequestMessage(HttpMethod.Get, ouiAddress);
                    using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();

                    var payload = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Diagnostics.Info("oui_download_completed", ("attempt", attempt), ("bytes", payload.Length));
                    return payload;
                }
                catch (OperationCanceledException)
                {
                    Diagnostics.Warning("oui_download_cancelled", "OUI download cancelled by caller.", ("attempt", attempt));
                    throw;
                }
                catch (Exception ex) when (attempt < retryCount)
                {
                    var backoff = TimeSpan.FromMilliseconds(250 * attempt * attempt);
                    Diagnostics.Warning("oui_download_retry", ex.Message, ("attempt", attempt), ("retryInMs", backoff.TotalMilliseconds));
                    await Task.Delay(backoff, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Diagnostics.Error("oui_download_failed", ex, "Failed to download OUI data after retries.", ("attempt", attempt), ("endpoint", ouiAddress));
                    throw new MacChangerException("Failed to download OUI vendor list from IEEE.", ex);
                }
            }

            throw new MacChangerException("Failed to download OUI vendor list from IEEE.");
        }

        private static List<Vendor> Parse(string oui)
        {
            Diagnostics.Info("oui_parse_start", ("contentLength", oui.Length));
            var vendors = new List<Vendor>();
            const RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant;
            var pattern = new Regex(@"^(\w{6})\s+\(base 16\)\t+(.+)$", options);
            foreach (Match item in pattern.Matches(oui))
            {
                vendors.Add(new Vendor(item.Groups[1].Value, item.Groups[2].Value));
            }

            Diagnostics.Info("oui_parse_completed", ("vendorCount", vendors.Count));
            return vendors;
        }

        private static int ReadIntSetting(string key, int defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];
            return int.TryParse(value, out var parsed) ? parsed : defaultValue;
        }
    }
}
