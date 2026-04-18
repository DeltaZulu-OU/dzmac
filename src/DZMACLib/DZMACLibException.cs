using System;
using System.Runtime.Serialization;

namespace DZMACLib
{
    /// <summary>
    ///     Library-specific exceptions
    /// </summary>
    public sealed class DZMACLibException : Exception
    {
        /// <inheritdoc />
        public DZMACLibException() : base()
        {
        }

        /// <inheritdoc />
        public DZMACLibException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public DZMACLibException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <inheritdoc />
        /// <exception cref="SerializationException"></exception>
        public DZMACLibException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}