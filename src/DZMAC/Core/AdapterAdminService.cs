#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace Dzmac.Gui.Core
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
        public System.Collections.Generic.IReadOnlyDictionary<string, string> Context { get; }

        public bool IsSuccess => Code == AdapterAdminResultCode.Success;

        public AdapterAdminResult(AdapterAdminResultCode code, string message, System.Collections.Generic.IReadOnlyDictionary<string, string>? context = null)
        {
            Code = code;
            Message = message;
            Context = context ?? new System.Collections.Generic.Dictionary<string, string>();
        }

        public static AdapterAdminResult Success(string message, params (string Key, string Value)[] context) =>
            new AdapterAdminResult(AdapterAdminResultCode.Success, message, ToDictionary(context));

        public static AdapterAdminResult Failed(AdapterAdminResultCode code, string message, params (string Key, string Value)[] context) =>
            new AdapterAdminResult(code, message, ToDictionary(context));

        private static System.Collections.Generic.Dictionary<string, string> ToDictionary((string Key, string Value)[] context)
        {
            var dict = new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);
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

        AdapterAdminResult SetRegistryMac(NetworkAdapter adapter, MacAddress macAddress, bool persistOriginalRecord = true);

        AdapterAdminResult ResetRegistryMac(NetworkAdapter adapter);

        AdapterAdminResult DeleteAdapterFromRegistry(NetworkAdapter adapter);

        Task<AdapterAdminResult> SetAdapterEnabledAsync(NetworkAdapter adapter, bool enabled, CancellationToken cancellationToken = default);

        Task<AdapterAdminResult> SetDhcpEnabledAsync(NetworkAdapter adapter, bool enabled, CancellationToken cancellationToken = default);

        Task<AdapterAdminResult> ReleaseDhcpLeaseAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default);

        Task<AdapterAdminResult> RenewDhcpLeaseAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default);

        Task<AdapterAdminResult> SetRegistryMacAsync(NetworkAdapter adapter, MacAddress macAddress, bool persistOriginalRecord = true, CancellationToken cancellationToken = default);

        Task<AdapterAdminResult> ResetRegistryMacAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default);

        Task<AdapterAdminResult> DeleteAdapterFromRegistryAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default);
    }

    public sealed class AdapterAdminService : IAdapterAdminService
    {
        private readonly IAdapterAdminCommandExecutor _executor;

        public AdapterAdminService()
            : this(new AdapterAdminCommandExecutor(AdapterAdminPolicy.FromConfig(ConfigReader.Current)))
        {
        }

        internal AdapterAdminService(IAdapterAdminCommandExecutor executor)
        {
            _executor = executor;
        }

        public AdapterAdminResult SetAdapterEnabled(NetworkAdapter adapter, bool enabled) => SetAdapterEnabledAsync(adapter, enabled).GetAwaiter().GetResult();

        public AdapterAdminResult SetDhcpEnabled(NetworkAdapter adapter, bool enabled) => SetDhcpEnabledAsync(adapter, enabled).GetAwaiter().GetResult();

        public AdapterAdminResult ReleaseDhcpLease(NetworkAdapter adapter) => ReleaseDhcpLeaseAsync(adapter).GetAwaiter().GetResult();

        public AdapterAdminResult RenewDhcpLease(NetworkAdapter adapter) => RenewDhcpLeaseAsync(adapter).GetAwaiter().GetResult();

        public AdapterAdminResult SetRegistryMac(NetworkAdapter adapter, MacAddress macAddress, bool persistOriginalRecord = true) => SetRegistryMacAsync(adapter, macAddress, persistOriginalRecord).GetAwaiter().GetResult();

        public AdapterAdminResult ResetRegistryMac(NetworkAdapter adapter) => ResetRegistryMacAsync(adapter).GetAwaiter().GetResult();

        public AdapterAdminResult DeleteAdapterFromRegistry(NetworkAdapter adapter) => DeleteAdapterFromRegistryAsync(adapter).GetAwaiter().GetResult();

        public Task<AdapterAdminResult> SetAdapterEnabledAsync(NetworkAdapter adapter, bool enabled, CancellationToken cancellationToken = default)
            => ExecuteAsync(adapter, enabled ? "adapter_enable" : "adapter_disable", () => enabled ? adapter.TryEnableAdapter() : adapter.TryDisableAdapter(), cancellationToken);

        public Task<AdapterAdminResult> SetDhcpEnabledAsync(NetworkAdapter adapter, bool enabled, CancellationToken cancellationToken = default)
            => ExecuteAsync(adapter, enabled ? "dhcp_enable" : "dhcp_disable", () => enabled ? adapter.TryDhcpEnable() : adapter.TryDhcpDisable(), cancellationToken);

        public Task<AdapterAdminResult> ReleaseDhcpLeaseAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default)
            => ExecuteAsync(adapter, "dhcp_release", () =>
            {
                var success = adapter.TryDhcpRelease(out var message);
                return (success, message);
            }, cancellationToken);

        public Task<AdapterAdminResult> RenewDhcpLeaseAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default)
            => ExecuteAsync(adapter, "dhcp_renew", () =>
            {
                var success = adapter.TryDhcpRenew(out var message);
                return (success, message);
            }, cancellationToken);

        public Task<AdapterAdminResult> SetRegistryMacAsync(NetworkAdapter adapter, MacAddress macAddress, bool persistOriginalRecord = true, CancellationToken cancellationToken = default)
            => ExecuteAsync(adapter, "registry_mac_set", () => adapter.TrySetRegistryMac(macAddress, persistOriginalRecord), cancellationToken);

        public Task<AdapterAdminResult> ResetRegistryMacAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default)
            => ExecuteAsync(adapter, "registry_mac_reset", () => adapter.TrySetRegistryMac(null), cancellationToken);

        public Task<AdapterAdminResult> DeleteAdapterFromRegistryAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default)
            => ExecuteAsync(adapter, "registry_adapter_delete", () => adapter.TryDeleteFromRegistry(), cancellationToken);

        private Task<AdapterAdminResult> ExecuteAsync(NetworkAdapter adapter, string operationName, System.Func<bool> operation, CancellationToken cancellationToken)
            => ExecuteAsync(adapter, operationName, () =>
            {
                var success = operation();
                return (success, success ? "OK" : "Operation returned false");
            }, cancellationToken);

        private Task<AdapterAdminResult> ExecuteAsync(NetworkAdapter adapter, string operationName, System.Func<(bool Success, string Message)> operation, CancellationToken cancellationToken)
        {
            if (adapter == null)
            {
                return Task.FromResult(AdapterAdminResult.Failed(AdapterAdminResultCode.InvalidArgument, "Adapter cannot be null."));
            }

            var command = new AdapterAdminCommand(operationName, adapter.Name, operation);
            return _executor.ExecuteAsync(command, cancellationToken);
        }
    }
}