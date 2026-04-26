#nullable enable

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Dzmac.Core
{
    internal sealed class AdapterAdminPolicy
    {
        public int TimeoutSeconds { get; }
        public int RetryCount { get; }

        public AdapterAdminPolicy(int timeoutSeconds, int retryCount)
        {
            TimeoutSeconds = Math.Max(1, timeoutSeconds);
            RetryCount = Math.Max(1, retryCount);
        }

        public static AdapterAdminPolicy FromConfig(IAppSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return new AdapterAdminPolicy(
                settings.GetInt(AppSettingKeys.AdminOperationTimeoutSeconds),
                settings.GetInt(AppSettingKeys.AdminOperationRetryCount));
        }
    }

    internal sealed class AdapterAdminCommand
    {
        private readonly Func<CancellationToken, (bool Success, string Message)> _operation;

        public string Name { get; }
        public string AdapterName { get; }

        public AdapterAdminCommand(string name, string adapterName, Func<CancellationToken, (bool Success, string Message)> operation)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Command name cannot be empty.", nameof(name));
            }

            Name = name;
            AdapterName = string.IsNullOrWhiteSpace(adapterName) ? "unknown" : adapterName;
            _operation = operation ?? throw new ArgumentNullException(nameof(operation));
        }

        public (bool Success, string Message) Execute(CancellationToken cancellationToken) => _operation(cancellationToken);
    }

    internal sealed class AdapterAdminCommandExecutor
    {
        private readonly AdapterAdminPolicy _policy;

        public AdapterAdminCommandExecutor(AdapterAdminPolicy policy)
        {
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public async Task<AdapterAdminResult> ExecuteAsync(AdapterAdminCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
            {
                return AdapterAdminResult.Failed(AdapterAdminResultCode.InvalidArgument, "Command cannot be null.");
            }

            Exception? lastException = null;
            for (var attempt = 1; attempt <= _policy.RetryCount; attempt++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return AdapterAdminResult.Failed(
                        AdapterAdminResultCode.Timeout,
                        "Operation cancelled.",
                        ("operation", command.Name),
                        ("adapter", command.AdapterName));
                }

                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(TimeSpan.FromSeconds(_policy.TimeoutSeconds));

                var stopwatch = Stopwatch.StartNew();
                try
                {
                    var operationTask = Task.Run(() => command.Execute(timeoutCts.Token));
                    var (Success, Message) = await operationTask.ConfigureAwait(false);
                    stopwatch.Stop();

                    Diagnostics.Info("admin_operation_completed",
                        ("operation", command.Name),
                        ("adapter", command.AdapterName),
                        ("attempt", attempt),
                        ("durationMs", stopwatch.ElapsedMilliseconds),
                        ("success", Success));

                    if (Success)
                    {
                        return AdapterAdminResult.Success(
                            Message,
                            ("operation", command.Name),
                            ("adapter", command.AdapterName),
                            ("attempt", attempt.ToString()));
                    }

                    if (attempt >= _policy.RetryCount)
                    {
                        return AdapterAdminResult.Failed(
                            AdapterAdminResultCode.Failed,
                            Message,
                            ("operation", command.Name),
                            ("adapter", command.AdapterName),
                            ("attempt", attempt.ToString()));
                    }

                    Diagnostics.Warning("admin_operation_retry", Message, ("operation", command.Name), ("adapter", command.AdapterName), ("attempt", attempt));
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    return AdapterAdminResult.Failed(
                        AdapterAdminResultCode.Timeout,
                        "Operation cancelled.",
                        ("operation", command.Name),
                        ("adapter", command.AdapterName));
                }
                catch (OperationCanceledException)
                {
                    return AdapterAdminResult.Failed(
                        AdapterAdminResultCode.Timeout,
                        "Operation timed out.",
                        ("operation", command.Name),
                        ("adapter", command.AdapterName),
                        ("timeoutSeconds", _policy.TimeoutSeconds.ToString()));
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    lastException = ex;
                    Diagnostics.Warning("admin_operation_retry", ex.Message, ("operation", command.Name), ("adapter", command.AdapterName), ("attempt", attempt));
                    if (attempt >= _policy.RetryCount)
                    {
                        break;
                    }
                }
            }

            return AdapterAdminResult.Failed(
                AdapterAdminResultCode.Exception,
                lastException?.Message ?? "Operation failed.",
                ("operation", command.Name),
                ("adapter", command.AdapterName));
        }
    }

    internal enum AdapterAdminResultCode
    {
        Success,
        Failed,
        InvalidArgument,
        Timeout,
        Exception
    }

    internal sealed class AdapterAdminResult
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

    internal sealed class AdapterAdminService
    {
        private readonly AdapterAdminCommandExecutor _executor;

        public AdapterAdminService()
            : this(new AdapterAdminCommandExecutor(AdapterAdminPolicy.FromConfig(ConfigReader.Current)))
        {
        }

        internal AdapterAdminService(AdapterAdminCommandExecutor executor)
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
            => ExecuteAsync(adapter, "dhcp_release", _ =>
            {
                var success = adapter.TryDhcpRelease(out var message);
                return (success, message);
            }, cancellationToken);

        public Task<AdapterAdminResult> RenewDhcpLeaseAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default)
            => ExecuteAsync(adapter, "dhcp_renew", _ =>
            {
                var success = adapter.TryDhcpRenew(out var message);
                return (success, message);
            }, cancellationToken);

        public Task<AdapterAdminResult> SetRegistryMacAsync(NetworkAdapter adapter, MacAddress macAddress, bool persistOriginalRecord = true, CancellationToken cancellationToken = default)
            => ExecuteAsync(adapter, "registry_mac_set", () => adapter.TrySetRegistryMac(macAddress, persistOriginalRecord), cancellationToken);

        public Task<AdapterAdminResult> ResetRegistryMacAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default)
            => ExecuteAsync(adapter, "registry_mac_reset", () => adapter.TrySetRegistryMac(null), cancellationToken);

        public Task<AdapterAdminResult> DeleteAdapterFromRegistryAsync(NetworkAdapter adapter, CancellationToken cancellationToken = default)
            => ExecuteAsync(adapter, "registry_adapter_delete", _ => adapter.TryDeleteFromRegistry(), cancellationToken);

        private Task<AdapterAdminResult> ExecuteAsync(NetworkAdapter adapter, string operationName, Func<bool> operation, CancellationToken cancellationToken)
            => ExecuteAsync(adapter, operationName, _ =>
            {
                var success = operation();
                return (success, success ? "OK" : "Operation returned false");
            }, cancellationToken);

        private Task<AdapterAdminResult> ExecuteAsync(NetworkAdapter adapter, string operationName, Func<CancellationToken, (bool Success, string Message)> operation, CancellationToken cancellationToken)
        {
            if (adapter == null)
            {
                return Task.FromResult(AdapterAdminResult.Failed(AdapterAdminResultCode.InvalidArgument, "Adapter cannot be null."));
            }

            var command = new AdapterAdminCommand(operationName, adapter.Name, token =>
            {
                token.ThrowIfCancellationRequested();
                var result = operation(token);
                token.ThrowIfCancellationRequested();
                return result;
            });
            return _executor.ExecuteAsync(command, cancellationToken);
        }
    }
}
