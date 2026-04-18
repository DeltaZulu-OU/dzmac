#nullable enable

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DZMACLib
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
            var ouiAddress = ConfigReader.Current.GetString(AppSettingKeys.OuiEndpoint);
            if (string.IsNullOrWhiteSpace(ouiAddress))
            {
                ouiAddress = DefaultOuiAddress;
            }

            var timeoutSeconds = Math.Max(1, ConfigReader.Current.GetInt(AppSettingKeys.OuiDownloadTimeoutSeconds));
            if (timeoutSeconds <= 0)
            {
                timeoutSeconds = DefaultTimeoutSeconds;
            }

            var retryCount = Math.Max(1, ConfigReader.Current.GetInt(AppSettingKeys.OuiDownloadRetryCount));
            if (retryCount <= 0)
            {
                retryCount = DefaultRetryCount;
            }

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
                    throw new DZMACLibException("Failed to download OUI vendor list from IEEE.", ex);
                }
            }

            throw new DZMACLibException("Failed to download OUI vendor list from IEEE.");
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
    }
}
