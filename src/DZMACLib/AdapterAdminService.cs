#nullable enable

using System;
using System.Collections.Generic;
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
    }

    public sealed class AdapterAdminService : IAdapterAdminService
    {
        public AdapterAdminResult SetAdapterEnabled(NetworkAdapter adapter, bool enabled) => Execute(adapter, enabled ? "adapter_enable" : "adapter_disable", () => enabled ? adapter.TryEnableAdapter() : adapter.TryDisableAdapter());

        public AdapterAdminResult SetDhcpEnabled(NetworkAdapter adapter, bool enabled) => Execute(adapter, enabled ? "dhcp_enable" : "dhcp_disable", () => enabled ? adapter.TryDhcpEnable() : adapter.TryDhcpDisable());

        public AdapterAdminResult ReleaseDhcpLease(NetworkAdapter adapter) => Execute(adapter, "dhcp_release", () =>
                                                                                       {
                                                                                           var success = adapter.TryDhcpRelease(out var message);
                                                                                           return (success, message);
                                                                                       });

        public AdapterAdminResult RenewDhcpLease(NetworkAdapter adapter) => Execute(adapter, "dhcp_renew", () =>
                                                                                     {
                                                                                         var success = adapter.TryDhcpRenew(out var message);
                                                                                         return (success, message);
                                                                                     });

        public AdapterAdminResult SetRegistryMac(NetworkAdapter adapter, MacAddress macAddress) => Execute(adapter, "registry_mac_set", () => adapter.TrySetRegistryMac(macAddress));

        public AdapterAdminResult ResetRegistryMac(NetworkAdapter adapter) => Execute(adapter, "registry_mac_reset", () => adapter.TrySetRegistryMac(null));

        private static AdapterAdminResult Execute(NetworkAdapter adapter, string operationName, Func<bool> operation) => Execute(adapter, operationName, () =>
                                                                                                                                  {
                                                                                                                                      var success = operation();
                                                                                                                                      return (success, success ? "OK" : "Operation returned false");
                                                                                                                                  });

        private static AdapterAdminResult Execute(NetworkAdapter adapter, string operationName, Func<(bool Success, string Message)> operation)
        {
            if (adapter == null)
            {
                return AdapterAdminResult.Failed(AdapterAdminResultCode.InvalidArgument, "Adapter cannot be null.");
            }

            var timeoutSeconds = Math.Max(1, ConfigReader.Current.GetInt(AppSettingKeys.AdminOperationTimeoutSeconds));
            try
            {
                var task = Task.Run(operation);
                if (!task.Wait(TimeSpan.FromSeconds(timeoutSeconds)))
                {
                    return AdapterAdminResult.Failed(AdapterAdminResultCode.Timeout, "Operation timed out.", ("operation", operationName), ("adapter", adapter.Name));
                }

                var (Success, Message) = task.Result;
                return Success
                    ? AdapterAdminResult.Success(Message, ("operation", operationName), ("adapter", adapter.Name))
                    : AdapterAdminResult.Failed(AdapterAdminResultCode.Failed, Message, ("operation", operationName), ("adapter", adapter.Name));
            }
            catch (AggregateException ex)
            {
                var inner = ex.InnerException ?? ex;
                return AdapterAdminResult.Failed(AdapterAdminResultCode.Exception, inner.Message, ("operation", operationName), ("adapter", adapter.Name));
            }
            catch (Exception ex)
            {
                return AdapterAdminResult.Failed(AdapterAdminResultCode.Exception, ex.Message, ("operation", operationName), ("adapter", adapter.Name));
            }
        }
    }
}
