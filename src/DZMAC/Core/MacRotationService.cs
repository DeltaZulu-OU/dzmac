#nullable enable

using System;
using System.Security;
using System.Threading;

namespace Dzmac.Core
{
    internal static class MacRotationService
    {
        private const int MacPollingIntervalMs = 200;
        private const int DisablePollTimeoutMs = 10000;
        private const int EnablePollTimeoutMs = 10000;
        private const int AttemptWatchdogTimeoutMs = 20000;
        private static readonly string[] LaaPrefixes = { "02", "06", "0A", "0E" };

        public static (bool Success, string Message) TryRotateMac(NetworkAdapter adapter, MacAddress target, bool persistOriginalRecord, IProgress<string> progress)
        {
            progress.Report("Starting MAC update...");

            try
            {
                progress.Report("Configuring Registry...");
                adapter.EnsureNetworkAddressRegistryParameter();
            }
            catch (UnauthorizedAccessException)
            {
                return (false, "Access Denied. Please run as Administrator. (ERR_REG_DENIED)");
            }
            catch (SecurityException)
            {
                return (false, "Access Denied. Please run as Administrator. (ERR_REG_DENIED)");
            }

            var suffix = target.ToString().Substring(2);
            foreach (var prefix in LaaPrefixes)
            {
                var attemptMac = new MacAddress(prefix + suffix);
                var attemptResult = TryApplyMacAttempt(adapter, attemptMac, persistOriginalRecord, progress);
                if (attemptResult.Success)
                {
                    return attemptResult;
                }

                if (attemptResult.Message.Contains("ERR_TIMEOUT"))
                {
                    RevertToFactoryState(adapter, progress);
                    return (false, "Hardware initialization timeout. Check Device Manager. (ERR_TIMEOUT)");
                }

                if (attemptResult.Message.Contains("ERR_REG_DENIED") || attemptResult.Message.Contains("ERR_WMI_FAIL"))
                {
                    return attemptResult;
                }
            }

            RevertToFactoryState(adapter, progress);
            return (false, "This device is hardware-locked against MAC spoofing. (ERR_HW_LOCKED)");
        }

        private static (bool Success, string Message) TryApplyMacAttempt(NetworkAdapter adapter, MacAddress target, bool persistOriginalRecord, IProgress<string> progress)
        {
            using var watchdogCts = new CancellationTokenSource(AttemptWatchdogTimeoutMs);
            var token = watchdogCts.Token;

            try
            {
                progress.Report("Configuring Registry...");
                adapter.TryUpdateRegistryMacValue(target, persistOriginalRecord);
            }
            catch (UnauthorizedAccessException)
            {
                return (false, "Access Denied. Please run as Administrator. (ERR_REG_DENIED)");
            }
            catch (SecurityException)
            {
                return (false, "Access Denied. Please run as Administrator. (ERR_REG_DENIED)");
            }

            try
            {
                progress.Report("Requesting adapter shutdown...");
                if (!adapter.TryDisableAdapter())
                {
                    return (false, "System refused to toggle hardware state. Check VPN/AV. (ERR_WMI_FAIL)");
                }

                var disableAttempts = 0;
                while (adapter.IsAdapterEnabled())
                {
                    token.ThrowIfCancellationRequested();
                    disableAttempts++;
                    progress.Report($"Disconnecting... [Attempt {disableAttempts}]");
                    Thread.Sleep(MacPollingIntervalMs);

                    if (disableAttempts * MacPollingIntervalMs >= DisablePollTimeoutMs)
                    {
                        return (false, "Hardware initialization timeout. Check Device Manager. (ERR_TIMEOUT)");
                    }
                }

                progress.Report("Requesting adapter start...");
                if (!adapter.TryEnableAdapter())
                {
                    return (false, "System refused to toggle hardware state. Check VPN/AV. (ERR_WMI_FAIL)");
                }

                var enableAttempts = 0;
                while (!adapter.IsAdapterEnabled())
                {
                    token.ThrowIfCancellationRequested();
                    enableAttempts++;
                    progress.Report("Initializing Hardware...");
                    Thread.Sleep(MacPollingIntervalMs);

                    if (enableAttempts * MacPollingIntervalMs >= EnablePollTimeoutMs)
                    {
                        return (false, "Hardware initialization timeout. Check Device Manager. (ERR_TIMEOUT)");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                return (false, "Hardware initialization timeout. Check Device Manager. (ERR_TIMEOUT)");
            }

            progress.Report("Verifying MAC change...");
            var liveMac = adapter.GetLiveLinkAddress();
            if (liveMac != null && liveMac.Equals(target))
            {
                return (true, "OK");
            }

            return (false, "Verification failed.");
        }

        private static void RevertToFactoryState(NetworkAdapter adapter, IProgress<string> progress)
        {
            try
            {
                adapter.TryUpdateRegistryMacValue(null, false);
                _ = adapter.TryDisableAdapter();
                Thread.Sleep(MacPollingIntervalMs);
                _ = adapter.TryEnableAdapter();
            }
            catch
            {
                // no-op: best-effort revert
            }

            progress.Report("Change failed. Reverted to factory settings.");
        }
    }
}
