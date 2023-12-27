using System;
using System.Runtime.Serialization;

namespace MacChanger
{
    /// <summary>
    ///     Library-specific exceptions
    /// </summary>
    public sealed class MacChangerException : Exception
    {
        /// <inheritdoc />
        public MacChangerException():base()
        {
        }

        /// <inheritdoc />
        public MacChangerException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public MacChangerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <inheritdoc />
        /// <exception cref="SerializationException"></exception>
        public MacChangerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
