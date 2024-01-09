using System;
using System.Text.RegularExpressions;


namespace MacChanger.Gui.Utils
{
    internal class MacFormatter : IFormatProvider, ICustomFormatter
    {
        private const string regex = "^(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})$";

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            // Check whether this is an appropriate callback
            if (!Equals(formatProvider))
            {
                return null;
            }

            // Set default format specifier
            if (string.IsNullOrEmpty(format))
            {
                format = "N";
            }

            var macString = arg.ToString();
            if (format == "N")
            {
                var replace = "$1:$2:$3:$4:$5:$6";
                macString = Regex.Replace(macString, regex, replace);
            }
            else
            {
                throw new FormatException(string.Format("The {0} format specifier is invalid.", format));
            }
            return macString;
        }
    }
}
