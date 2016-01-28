#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Framework
{
    /// <summary>
    /// Base exception for C4rm4x.WebApi.Framework
    /// </summary>
    public abstract class ApiException : Exception
    {
        /// <summary>
        /// Gets the code of this exception
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">The code of this exception</param>
        /// <param name="message">The error message that explains the reason for this exception</param>
        public ApiException(string code, string message)
            : this(code, message, null)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">The code of this exception</param>
        /// <param name="message">The error message that explains the reason for this exception</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        public ApiException(string code, string message, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }
    }
}
