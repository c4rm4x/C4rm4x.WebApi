#region using

using System;

#endregion

namespace C4rm4x.WebApi.Framework.Persistance
{
    /// <summary>
    /// Persistance exception
    /// </summary>
    public class PersistenceException : C4rm4xException
    {
        private new const string Code = "SYS_002";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public PersistenceException(string message) :
            this(message, null)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">Inner exception</param>
        public PersistenceException(string message, Exception innerException)
            : base(Code, message, innerException)
        { }
    }
}
