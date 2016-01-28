#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Framework.Validation
{
    /// <summary>
    /// Validation exceptions
    /// </summary>
    public class ValidationException : ApiException
    {
        private new const string Code = "SYS_001";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The error message that describes the validation error</param>
        public ValidationException(string message)
            : this(message, null)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The error message that describes the validation error</param>
        /// <param name="innerException">Inner exception</param>
        public ValidationException(string message, Exception innerException)
            : base(Code, message, innerException)
        { }
    }
}
