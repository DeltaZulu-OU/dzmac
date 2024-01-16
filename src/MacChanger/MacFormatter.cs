#nullable enable
using System;
using System.Text.RegularExpressions;

namespace MacChanger
{
    internal class MacFormatter : IFormatProvider, ICustomFormatter
    {
        private const string macReplace = "$1-$2-$3-$4-$5-$6";
        private static readonly Regex _regex = new Regex("^(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})$");

        public object? GetFormat(Type formatType) => formatType == typeof(ICustomFormatter)
            ? this
            : null;

        public string? Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (Equals(formatProvider))
            {
                try
                {
                    return _regex.Replace(arg.ToString(), macReplace);
                }
                catch (RegexMatchTimeoutException)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}