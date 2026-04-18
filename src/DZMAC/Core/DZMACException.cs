using System;
using System.Runtime.Serialization;

namespace Dzmac.Gui.Core
{
    /// <summary>
    ///     Library-specific exceptions
    /// </summary>
    public sealed class DZMACException : Exception
    {
        /// <inheritdoc />
        public DZMACException() : base()
        {
        }

        /// <inheritdoc />
        public DZMACException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public DZMACException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <inheritdoc />
        /// <exception cref="SerializationException"></exception>
        public DZMACException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}