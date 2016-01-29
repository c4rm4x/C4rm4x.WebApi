#region Using

using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Handlers
{
    /// <summary>
    /// Replaces an exception with a completely brand new one
    /// </summary>
    /// <remarks>
    /// This handler destroys the stack trace of the exception to be handled
    /// </remarks>
    public class ReplaceHandler :
        AbstractHandler, IExceptionHandler
    {
        /// <summary>
        /// Gets the type of the exception that will replace the original one
        /// </summary>
        public Type ReplaceExceptionType
        {
            get { return ExceptionType; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exceptionMessage">Error message of the new exception</param>
        /// <param name="replaceExceptionType">Type of the execption that will replace the original one</param>
        public ReplaceHandler(
            string exceptionMessage,
            Type replaceExceptionType)
            : base(exceptionMessage, replaceExceptionType)
        {
        }

        /// <summary>
        /// Handles the exception creating a new one with a new error message
        /// </summary>
        /// <param name="exception">The exception to be handled</param>
        /// <param name="exceptionMessage">The error message associated to the new one</param>
        /// <returns>A new instance of exception which replaces the original one</returns>
        protected override Exception HandleException(
            Exception exception,
            string exceptionMessage)
        {
            return CreateExceptionInstance(exceptionMessage);
        }
    }
}
