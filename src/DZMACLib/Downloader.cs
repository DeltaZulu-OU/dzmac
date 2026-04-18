#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DZMACLib
{
    internal static class Downloader
    {
        private const string DefaultOuiAddress = "https://standards-oui.ieee.org/oui/oui.txt";

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

            var retryCount = Math.Max(1, ConfigReader.Current.GetInt(AppSettingKeys.OuiDownloadRetryCount));

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
                    await VerifyPayloadIntegrityAsync(httpClient, payload, cancellationToken).ConfigureAwait(false);
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

        private static async Task VerifyPayloadIntegrityAsync(HttpClient httpClient, string payload, CancellationToken cancellationToken)
        {
            var manifestEndpoint = ConfigReader.Current.GetString(AppSettingKeys.OuiIntegrityManifestEndpoint);
            if (string.IsNullOrWhiteSpace(manifestEndpoint))
            {
                return;
            }

            if (!Uri.TryCreate(manifestEndpoint, UriKind.Absolute, out _))
            {
                Diagnostics.Warning("oui_integrity_manifest_invalid_uri", "Integrity manifest endpoint is not an absolute URI. Integrity verification skipped.", ("endpoint", manifestEndpoint));
                return;
            }

            Diagnostics.Info("oui_integrity_manifest_fetch_start", ("endpoint", manifestEndpoint));
            var manifestResponse = await httpClient.GetAsync(manifestEndpoint, cancellationToken).ConfigureAwait(false);
            manifestResponse.EnsureSuccessStatusCode();
            var manifestBody = await manifestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!TryExtractSha256(manifestBody, out var expectedHash))
            {
                Diagnostics.Error("oui_integrity_manifest_invalid", null, "Unable to parse SHA-256 value from configured manifest.", ("endpoint", manifestEndpoint));
                throw new DZMACLibException("Configured OUI integrity manifest does not contain a valid SHA-256 checksum.");
            }

            var actualHash = ComputeSha256(payload);
            if (!actualHash.Equals(expectedHash, StringComparison.OrdinalIgnoreCase))
            {
                Diagnostics.Error("oui_integrity_mismatch", null, "Downloaded OUI payload checksum does not match manifest.", ("expected", expectedHash), ("actual", actualHash));
                throw new DZMACLibException("Downloaded OUI payload failed integrity verification.");
            }

            Diagnostics.Info("oui_integrity_verified", ("sha256", actualHash));
        }

        internal static bool TryExtractSha256(string manifestBody, out string hash)
        {
            hash = string.Empty;
            if (string.IsNullOrWhiteSpace(manifestBody))
            {
                return false;
            }

            var match = Regex.Match(manifestBody, @"\b([A-Fa-f0-9]{64})\b", RegexOptions.CultureInvariant);
            if (!match.Success)
            {
                return false;
            }

            hash = match.Groups[1].Value.ToUpperInvariant();
            return true;
        }

        internal static string ComputeSha256(string content)
        {
            using var sha = SHA256.Create();
            var contentBytes = Encoding.UTF8.GetBytes(content);
            var digest = sha.ComputeHash(contentBytes);
            var builder = new StringBuilder(digest.Length * 2);
            foreach (var item in digest)
            {
                builder.Append(item.ToString("x2", CultureInfo.InvariantCulture));
            }

            return builder.ToString().ToUpperInvariant();
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
