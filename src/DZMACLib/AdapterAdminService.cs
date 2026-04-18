#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DZMACLib
{
    public enum AdapterAdminResultCode
    {
        Success,
        Failed,
        InvalidArgument,
        Timeout,
        Exception
    }

    public sealed class AdapterAdminResult
    {
        public AdapterAdminResultCode Code { get; }
        public string Message { get; }
        public IReadOnlyDictionary<string, string> Context { get; }

        public bool IsSuccess => Code == AdapterAdminResultCode.Success;

        public AdapterAdminResult(AdapterAdminResultCode code, string message, IReadOnlyDictionary<string, string>? context = null)
        {
            Code = code;
            Message = message;
            Context = context ?? new Dictionary<string, string>();
        }

        public static AdapterAdminResult Success(string message, params (string Key, string Value)[] context) =>
            new AdapterAdminResult(AdapterAdminResultCode.Success, message, ToDictionary(context));

        public static AdapterAdminResult Failed(AdapterAdminResultCode code, string message, params (string Key, string Value)[] context) =>
            new AdapterAdminResult(code, message, ToDictionary(context));

        private static Dictionary<string, string> ToDictionary((string Key, string Value)[] context)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var (Key, Value) in context)
            {
                dict[Key] = Value;
            }

            return dict;
        }
    }

    public interface IAdapterAdminService
    {
        AdapterAdminResult SetAdapterEnabled(NetworkAdapter adapter, bool enabled);
        AdapterAdminResult SetDhcpEnabled(NetworkAdapter adapter, bool enabled);
        AdapterAdminResult ReleaseDhcpLease(NetworkAdapter adapter);
        AdapterAdminResult RenewDhcpLease(NetworkAdapter adapter);
        AdapterAdminResult SetRegistryMac(NetworkAdapter adapter, MacAddress macAddress);
        AdapterAdminResult ResetRegistryMac(NetworkAdapter adapter);

        Task<AdapterAdminResult> SetAdapterEnabledAsync(NetworkAdapter adapter, bool enabled, CancellationToken cancellationToken = default);
        Task<AdapterAdminResult> SetDhcpEnabledAsync(NetworkAdapter adapter, bool enabled, CancellationToken cancellationToken = default);
        Task<AdapterAdminResult> ReleaseDhcpLeaseAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default);
        Task<AdapterAdminResult> RenewDhcpLeaseAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default);
        Task<AdapterAdminResult> SetRegistryMacAsync(NetworkAdapter adapter, MacAddress macAddress, CancellationToken cancellationToken = default);
        Task<AdapterAdminResult> ResetRegistryMacAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default);
    }

    public sealed class AdapterAdminService : IAdapterAdminService
    {
        public AdapterAdminResult SetAdapterEnabled(NetworkAdapter adapter, bool enabled) => SetAdapterEnabledAsync(adapter, enabled).GetAwaiter().GetResult();

        public AdapterAdminResult SetDhcpEnabled(NetworkAdapter adapter, bool enabled) => SetDhcpEnabledAsync(adapter, enabled).GetAwaiter().GetResult();

        public AdapterAdminResult ReleaseDhcpLease(NetworkAdapter adapter) => ReleaseDhcpLeaseAsync(adapter).GetAwaiter().GetResult();

        public AdapterAdminResult RenewDhcpLease(NetworkAdapter adapter) => RenewDhcpLeaseAsync(adapter).GetAwaiter().GetResult();

        public AdapterAdminResult SetRegistryMac(NetworkAdapter adapter, MacAddress macAddress) => SetRegistryMacAsync(adapter, macAddress).GetAwaiter().GetResult();

        public AdapterAdminResult ResetRegistryMac(NetworkAdapter adapter) => ResetRegistryMacAsync(adapter).GetAwaiter().GetResult();

        public Task<AdapterAdminResult> SetAdapterEnabledAsync(NetworkAdapter adapter, bool enabled, CancellationToken cancellationToken = default) => ExecuteAsync(adapter, enabled ? "adapter_enable" : "adapter_disable", () => enabled ? adapter.TryEnableAdapter() : adapter.TryDisableAdapter(), cancellationToken);

        public Task<AdapterAdminResult> SetDhcpEnabledAsync(NetworkAdapter adapter, bool enabled, CancellationToken cancellationToken = default) => ExecuteAsync(adapter, enabled ? "dhcp_enable" : "dhcp_disable", () => enabled ? adapter.TryDhcpEnable() : adapter.TryDhcpDisable(), cancellationToken);

        public Task<AdapterAdminResult> ReleaseDhcpLeaseAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default) => ExecuteAsync(adapter, "dhcp_release", () =>
        {
            var success = adapter.TryDhcpRelease(out var message);
            return (success, message);
        }, cancellationToken);

        public Task<AdapterAdminResult> RenewDhcpLeaseAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default) => ExecuteAsync(adapter, "dhcp_renew", () =>
        {
            var success = adapter.TryDhcpRenew(out var message);
            return (success, message);
        }, cancellationToken);

        public Task<AdapterAdminResult> SetRegistryMacAsync(NetworkAdapter adapter, MacAddress macAddress, CancellationToken cancellationToken = default) => ExecuteAsync(adapter, "registry_mac_set", () => adapter.TrySetRegistryMac(macAddress), cancellationToken);

        public Task<AdapterAdminResult> ResetRegistryMacAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default) => ExecuteAsync(adapter, "registry_mac_reset", () => adapter.TrySetRegistryMac(null), cancellationToken);

        private static Task<AdapterAdminResult> ExecuteAsync(NetworkAdapter adapter, string operationName, Func<bool> operation, CancellationToken cancellationToken) => ExecuteAsync(adapter, operationName, () =>
        {
            var success = operation();
            return (success, success ? "OK" : "Operation returned false");
        }, cancellationToken);

        private static async Task<AdapterAdminResult> ExecuteAsync(NetworkAdapter adapter, string operationName, Func<(bool Success, string Message)> operation, CancellationToken cancellationToken)
        {
            if (adapter == null)
            {
                return AdapterAdminResult.Failed(AdapterAdminResultCode.InvalidArgument, "Adapter cannot be null.");
            }

            var timeoutSeconds = Math.Max(1, ConfigReader.Current.GetInt(AppSettingKeys.AdminOperationTimeoutSeconds));
            var retryCount = Math.Max(1, ConfigReader.Current.GetInt(AppSettingKeys.AdminOperationRetryCount));

            Exception? lastException = null;
            for (var attempt = 1; attempt <= retryCount; attempt++)
            {
                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));

                var stopwatch = Stopwatch.StartNew();
                try
                {
                    var result = await Task.Run(operation, timeoutCts.Token).ConfigureAwait(false);
                    stopwatch.Stop();

                    Diagnostics.Info("admin_operation_completed",
                        ("operation", operationName),
                        ("adapter", adapter.Name),
                        ("attempt", attempt),
                        ("durationMs", stopwatch.ElapsedMilliseconds),
                        ("success", result.Success));

                    if (result.Success)
                    {
                        return AdapterAdminResult.Success(result.Message, ("operation", operationName), ("adapter", adapter.Name), ("attempt", attempt.ToString()));
                    }

                    if (attempt >= retryCount)
                    {
                        return AdapterAdminResult.Failed(AdapterAdminResultCode.Failed, result.Message, ("operation", operationName), ("adapter", adapter.Name), ("attempt", attempt.ToString()));
                    }

                    Diagnostics.Warning("admin_operation_retry", result.Message, ("operation", operationName), ("adapter", adapter.Name), ("attempt", attempt));
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    return AdapterAdminResult.Failed(AdapterAdminResultCode.Timeout, "Operation cancelled.", ("operation", operationName), ("adapter", adapter.Name));
                }
                catch (OperationCanceledException)
                {
                    return AdapterAdminResult.Failed(AdapterAdminResultCode.Timeout, "Operation timed out.", ("operation", operationName), ("adapter", adapter.Name), ("timeoutSeconds", timeoutSeconds.ToString()));
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    lastException = ex;
                    Diagnostics.Warning("admin_operation_retry", ex.Message, ("operation", operationName), ("adapter", adapter.Name), ("attempt", attempt));
                    if (attempt >= retryCount)
                    {
                        break;
                    }
                }
            }

            return AdapterAdminResult.Failed(AdapterAdminResultCode.Exception, lastException?.Message ?? "Operation failed.", ("operation", operationName), ("adapter", adapter.Name));
        }
    }
}
