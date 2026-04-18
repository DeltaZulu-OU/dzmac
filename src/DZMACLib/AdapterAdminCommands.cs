#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DZMACLib
{
    public interface IAdapterAdminCommand
    {
        string Name { get; }
        string AdapterName { get; }
        (bool Success, string Message) Execute();
    }

    public sealed class AdapterAdminPolicy
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

    public sealed class AdapterAdminCommand : IAdapterAdminCommand
    {
        private readonly Func<(bool Success, string Message)> _operation;

        public string Name { get; }
        public string AdapterName { get; }

        public AdapterAdminCommand(string name, string adapterName, Func<(bool Success, string Message)> operation)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Command name cannot be empty.", nameof(name));
            }

            Name = name;
            AdapterName = string.IsNullOrWhiteSpace(adapterName) ? "unknown" : adapterName;
            _operation = operation ?? throw new ArgumentNullException(nameof(operation));
        }

        public (bool Success, string Message) Execute() => _operation();
    }

    public interface IAdapterAdminCommandExecutor
    {
        Task<AdapterAdminResult> ExecuteAsync(IAdapterAdminCommand command, CancellationToken cancellationToken = default);
    }

    public sealed class AdapterAdminCommandExecutor : IAdapterAdminCommandExecutor
    {
        private readonly AdapterAdminPolicy _policy;

        public AdapterAdminCommandExecutor(AdapterAdminPolicy policy)
        {
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public async Task<AdapterAdminResult> ExecuteAsync(IAdapterAdminCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
            {
                return AdapterAdminResult.Failed(AdapterAdminResultCode.InvalidArgument, "Command cannot be null.");
            }

            Exception? lastException = null;
            for (var attempt = 1; attempt <= _policy.RetryCount; attempt++)
            {
                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(TimeSpan.FromSeconds(_policy.TimeoutSeconds));

                var stopwatch = Stopwatch.StartNew();
                try
                {
                    var result = await Task.Run(command.Execute, timeoutCts.Token).ConfigureAwait(false);
                    stopwatch.Stop();

                    Diagnostics.Info("admin_operation_completed",
                        ("operation", command.Name),
                        ("adapter", command.AdapterName),
                        ("attempt", attempt),
                        ("durationMs", stopwatch.ElapsedMilliseconds),
                        ("success", result.Success));

                    if (result.Success)
                    {
                        return AdapterAdminResult.Success(
                            result.Message,
                            ("operation", command.Name),
                            ("adapter", command.AdapterName),
                            ("attempt", attempt.ToString()));
                    }

                    if (attempt >= _policy.RetryCount)
                    {
                        return AdapterAdminResult.Failed(
                            AdapterAdminResultCode.Failed,
                            result.Message,
                            ("operation", command.Name),
                            ("adapter", command.AdapterName),
                            ("attempt", attempt.ToString()));
                    }

                    Diagnostics.Warning("admin_operation_retry", result.Message, ("operation", command.Name), ("adapter", command.AdapterName), ("attempt", attempt));
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
}
