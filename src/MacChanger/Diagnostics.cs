#nullable enable

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace MacChanger
{
    internal static class Diagnostics
    {
        private static readonly bool VerboseEnabled = ResolveVerboseEnabled();

        public static void Info(string eventName, params (string Key, object? Value)[] context) =>
            Write(TraceEventType.Information, eventName, null, context);

        public static void Warning(string eventName, string? message = null, params (string Key, object? Value)[] context) =>
            Write(TraceEventType.Warning, eventName, message, context);

        public static void Error(string eventName, Exception? exception = null, string? message = null, params (string Key, object? Value)[] context) =>
            Write(TraceEventType.Error, eventName, message ?? exception?.Message, context, exception);

        private static void Write(TraceEventType level, string eventName, string? message, IEnumerable<(string Key, object? Value)> context, Exception? exception = null)
        {
            if (!VerboseEnabled && level == TraceEventType.Information)
            {
                return;
            }

            var contextText = string.Join(", ", context.Select(item => $"{item.Key}={NormalizeValue(item.Value)}"));
            var logMessage = $"event={eventName}";

            if (!string.IsNullOrWhiteSpace(message))
            {
                logMessage += $", message=\"{message}\"";
            }

            if (!string.IsNullOrWhiteSpace(contextText))
            {
                logMessage += $", {contextText}";
            }

            if (exception != null)
            {
                logMessage += $", exception={exception.GetType().Name}";
            }

            switch (level)
            {
                case TraceEventType.Warning:
                    Trace.TraceWarning(logMessage);
                    break;
                case TraceEventType.Error:
                case TraceEventType.Critical:
                    Trace.TraceError(logMessage);
                    break;
                default:
                    Trace.TraceInformation(logMessage);
                    break;
            }
        }

        private static bool ResolveVerboseEnabled()
        {
            var env = Environment.GetEnvironmentVariable("MACCHANGER_VERBOSE_DIAGNOSTICS");
            if (bool.TryParse(env, out var envEnabled))
            {
                return envEnabled;
            }

            var appSetting = ConfigurationManager.AppSettings["MacChanger.VerboseDiagnostics"];
            return bool.TryParse(appSetting, out var appSettingEnabled) && appSettingEnabled;
        }

        private static string NormalizeValue(object? value)
        {
            if (value == null)
            {
                return "null";
            }

            var text = value.ToString();
            return string.IsNullOrWhiteSpace(text) ? "empty" : text.Replace("\"", "'");
        }
    }
}
