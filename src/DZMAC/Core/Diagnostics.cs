using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dzmac.Core
{
    internal static class Diagnostics
    {
        private const string EventLogName = "Application";
        private const string EventLogSource = "DZMAC";
        private const int PendingQueueLimit = 512;
        private const int EventLogInitRetryDelaySeconds = 30;

        private static readonly bool VerboseEnabled = ResolveVerboseEnabled();
        private static readonly ConcurrentQueue<PendingEntry> PendingEntries = new ConcurrentQueue<PendingEntry>();

        private static int _pendingCount;
        private static int _eventLogInitStarted;
        private static long _nextEventLogInitRetryTicks;
        private static volatile bool _eventLogReady;
        private static int _queueOverflowWarned;

        public static void Info(string eventName, params (string Key, object? Value)[] context) =>
            Write(TraceEventType.Information, eventName, null, context);

        public static void Debug(string eventName, params (string Key, object? Value)[] context) =>
            Write(TraceEventType.Verbose, eventName, null, context, null, writeToEventLog: false);

        public static void Warning(string eventName, string? message = null, params (string Key, object? Value)[] context) =>
            Write(TraceEventType.Warning, eventName, message, context);

        public static void Error(string eventName, Exception? exception = null, string? message = null, params (string Key, object? Value)[] context) =>
            Write(TraceEventType.Error, eventName, message ?? exception?.Message, context, exception);

        private static void Write(TraceEventType level, string eventName, string? message, IEnumerable<(string Key, object? Value)> context, Exception? exception = null, bool writeToEventLog = true)
        {
            var definition = DiagnosticsEventCatalog.Resolve(eventName);
            var logMessage = BuildMessage(definition.Template, eventName, message, context, exception);

            WriteToTrace(level, logMessage);

            if (!writeToEventLog)
            {
                return;
            }

            EnsureEventLogInitialization();

            var entry = new PendingEntry(level, definition.Id, logMessage);
            if (_eventLogReady)
            {
                if (!TryWriteToEventLog(entry))
                {
                    EnqueuePending(entry);
                }

                return;
            }

            EnqueuePending(entry);
        }

        private static string BuildMessage(string template, string eventName, string? message, IEnumerable<(string Key, object? Value)> context, Exception? exception)
        {
            var sb = new StringBuilder();
            sb.AppendLine(template);
            sb.Append($"  Event: {eventName}");

            if (!string.IsNullOrWhiteSpace(message))
            {
                sb.AppendLine();
                sb.Append($"  Message: {message!.Replace("\"", "'")}");
            }

            foreach (var item in context)
            {
                sb.AppendLine();
                sb.Append($"  {item.Key}: {NormalizeValue(item.Value)}");
            }

            if (exception is not null)
            {
                sb.AppendLine();
                sb.Append($"  Exception: {exception.GetType().Name}");
                sb.AppendLine();
                sb.Append($"  Details: {NormalizeValue(exception)}");
            }

            return sb.ToString();
        }

        private static void EnsureEventLogInitialization()
        {
            if (_eventLogReady)
            {
                return;
            }

            var retryAfterTicks = Interlocked.Read(ref _nextEventLogInitRetryTicks);
            if (retryAfterTicks > DateTime.UtcNow.Ticks)
            {
                return;
            }

            if (Interlocked.CompareExchange(ref _eventLogInitStarted, 1, 0) == 1)
            {
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    EnsureEventSourceRegistered();
                    _eventLogReady = true;
                    FlushPendingEntries();
                }
                catch (Exception ex)
                {
                    Trace.TraceWarning($"Event log initialization failed.\n  Event: event_log_initialization_failed\n  Source: {EventLogSource}\n  Message: {NormalizeValue(ex.Message)}");
                    _eventLogReady = false;
                    var retryAt = DateTime.UtcNow.AddSeconds(EventLogInitRetryDelaySeconds);
                    Interlocked.Exchange(ref _nextEventLogInitRetryTicks, retryAt.Ticks);
                }
                finally
                {
                    Interlocked.Exchange(ref _eventLogInitStarted, 0);
                }
            });
        }

        private static void EnsureEventSourceRegistered()
        {
            if (!EventLog.SourceExists(EventLogSource))
            {
                var sourceData = new EventSourceCreationData(EventLogSource, EventLogName);
                EventLog.CreateEventSource(sourceData);
            }

            var sourceLogName = EventLog.LogNameFromSourceName(EventLogSource, ".");
            if (!string.Equals(sourceLogName, EventLogName, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Event source '{EventLogSource}' is registered to '{sourceLogName}', expected '{EventLogName}'.");
            }
        }

        private static void FlushPendingEntries()
        {
            while (_eventLogReady && PendingEntries.TryDequeue(out var entry))
            {
                Interlocked.Decrement(ref _pendingCount);
                if (!TryWriteToEventLog(entry))
                {
                    EnqueuePending(entry);
                    break;
                }
            }
        }

        private static void EnqueuePending(PendingEntry entry)
        {
            PendingEntries.Enqueue(entry);
            var count = Interlocked.Increment(ref _pendingCount);
            if (count <= PendingQueueLimit)
            {
                return;
            }

            if (Interlocked.CompareExchange(ref _queueOverflowWarned, 1, 0) == 0)
            {
                Trace.TraceWarning($"Diagnostics queue overflow: oldest entries will be dropped.\n  Event: diagnostics_queue_overflow\n  Limit: {PendingQueueLimit}");
            }

            if (PendingEntries.TryDequeue(out _))
            {
                Interlocked.Decrement(ref _pendingCount);
            }
        }

        private static bool TryWriteToEventLog(PendingEntry entry)
        {
            try
            {
                EventLog.WriteEntry(EventLogSource, entry.Message, ToEventLogType(entry.Level), entry.EventId);
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceWarning($"Event log write failed.\n  Event: event_log_write_failed\n  Source: {EventLogSource}\n  Id: {entry.EventId}\n  Message: {NormalizeValue(ex.Message)}");
                _eventLogReady = false;
                EnsureEventLogInitialization();
                return false;
            }
        }

        private static void WriteToTrace(TraceEventType level, string message)
        {
            switch (level)
            {
                case TraceEventType.Warning:
                    Trace.TraceWarning(message);
                    break;

                case TraceEventType.Verbose:
                    if (!VerboseEnabled)
                    {
                        return;
                    }

                    Trace.WriteLine($"DEBUG {message}");
                    break;

                case TraceEventType.Error:
                case TraceEventType.Critical:
                    Trace.TraceError(message);
                    break;

                default:
                    Trace.TraceInformation(message);
                    break;
            }
        }

        private static EventLogEntryType ToEventLogType(TraceEventType level)
        {
            switch (level)
            {
                case TraceEventType.Warning:
                    return EventLogEntryType.Warning;

                case TraceEventType.Error:
                case TraceEventType.Critical:
                    return EventLogEntryType.Error;

                default:
                    return EventLogEntryType.Information;
            }
        }

        private static bool ResolveVerboseEnabled()
        {
            var env = Environment.GetEnvironmentVariable("Dzmac.Core_VERBOSE_DIAGNOSTICS");
            if (bool.TryParse(env, out var envEnabled))
            {
                return envEnabled;
            }

            return ConfigReader.Current.GetBool(AppSettingKeys.VerboseDiagnostics);
        }

        private static string NormalizeValue(object? value)
        {
            if (value is null)
            {
                return "null";
            }

            var text = value.ToString();
            return string.IsNullOrWhiteSpace(text) ? "empty" : text.Replace("\"", "'");
        }

        private readonly struct PendingEntry
        {
            public PendingEntry(TraceEventType level, int eventId, string message)
            {
                Level = level;
                EventId = eventId;
                Message = message;
            }

            public TraceEventType Level { get; }

            public int EventId { get; }

            public string Message { get; }
        }
    }
}