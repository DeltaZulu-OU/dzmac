using System;
using System.Security;
using System.Threading;

namespace Dzmac.Core
{
    internal static class MacRotationService
    {
        private enum MacApplyResultCode
        {
            Ok,
            Timeout,
            RegistryDenied,
            WmiFailure,
            VerificationFailed
        }

        private struct MacApplyResult
        {
            public MacApplyResult(bool success, string message, MacApplyResultCode code)
            {
                Success = success;
                Message = message;
                Code = code;
            }

            public bool Success { get; }
            public string Message { get; }
            public MacApplyResultCode Code { get; }
        }

        private const int MacPollingIntervalMs = 200;
        private const int DisablePollTimeoutMs = 10000;
        private const int EnablePollTimeoutMs = 10000;
        private const int AttemptWatchdogTimeoutMs = 20000;
        private static readonly string[] LaaPrefixes = { "02", "06", "0A", "0E" };

        public static (bool Success, string Message) TryRotateMac(NetworkAdapter adapter, MacAddress target, bool persistOriginalRecord, IProgress<string> progress)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException(nameof(adapter));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (progress == null)
            {
                throw new ArgumentNullException(nameof(progress));
            }

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
                    return (true, attemptResult.Message);
                }

                if (attemptResult.Code == MacApplyResultCode.Timeout)
                {
                    RevertToFactoryState(adapter, progress);
                    return (false, "Hardware initialization timeout. Check Device Manager. (ERR_TIMEOUT)");
                }

                if (attemptResult.Code == MacApplyResultCode.RegistryDenied || attemptResult.Code == MacApplyResultCode.WmiFailure)
                {
                    return (false, attemptResult.Message);
                }
            }

            RevertToFactoryState(adapter, progress);
            return (false, "This device is hardware-locked against MAC spoofing. (ERR_HW_LOCKED)");
        }

        private static MacApplyResult TryApplyMacAttempt(NetworkAdapter adapter, MacAddress target, bool persistOriginalRecord, IProgress<string> progress)
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
                return new MacApplyResult(false, "Access Denied. Please run as Administrator. (ERR_REG_DENIED)", MacApplyResultCode.RegistryDenied);
            }
            catch (SecurityException)
            {
                return new MacApplyResult(false, "Access Denied. Please run as Administrator. (ERR_REG_DENIED)", MacApplyResultCode.RegistryDenied);
            }

            try
            {
                progress.Report("Requesting adapter shutdown...");
                if (!adapter.TryDisableAdapter())
                {
                    return new MacApplyResult(false, "System refused to toggle hardware state. Check VPN/AV. (ERR_WMI_FAIL)", MacApplyResultCode.WmiFailure);
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
                        return new MacApplyResult(false, "Hardware initialization timeout. Check Device Manager. (ERR_TIMEOUT)", MacApplyResultCode.Timeout);
                    }
                }

                progress.Report("Requesting adapter start...");
                if (!adapter.TryEnableAdapter())
                {
                    return new MacApplyResult(false, "System refused to toggle hardware state. Check VPN/AV. (ERR_WMI_FAIL)", MacApplyResultCode.WmiFailure);
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
                        return new MacApplyResult(false, "Hardware initialization timeout. Check Device Manager. (ERR_TIMEOUT)", MacApplyResultCode.Timeout);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                return new MacApplyResult(false, "Hardware initialization timeout. Check Device Manager. (ERR_TIMEOUT)", MacApplyResultCode.Timeout);
            }

            progress.Report("Verifying MAC change...");
            var liveMac = adapter.GetLiveLinkAddress();
            if (liveMac != null && liveMac.Equals(target))
            {
                return new MacApplyResult(true, "OK", MacApplyResultCode.Ok);
            }

            RevertToFactoryState(adapter, progress);
            return new MacApplyResult(false, "Verification failed. Reverted to factory settings.", MacApplyResultCode.VerificationFailed);
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
            catch (Exception ex)
            {
                Diagnostics.Warning("mac_revert_failed", ex.Message, ("adapter", adapter?.Name ?? "unknown"));
            }

            progress.Report("Change failed. Reverted to factory settings.");
        }

    }
}
