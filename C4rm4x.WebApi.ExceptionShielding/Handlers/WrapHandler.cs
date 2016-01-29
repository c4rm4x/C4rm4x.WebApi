#region Using

using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Handlers
{
    /// <summary>
    /// Wraps exception to be handled with a new one
    /// </summary>
    /// <remarks>
    /// The exception to be handled will be set as the InnerException of the new one
    /// </remarks>
    public class WrapHandler :
        AbstractHandler, IExceptionHandler
    {
        /// <summary>
        /// Gets the type of the exception that will wrap the original one
        /// </summary>
        public Type WrapExceptionType
        {
            get { return ExceptionType; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exceptionMessage">Error message of the new exception</param>
        /// <param name="wrapExceptionType">Type of the exception that will wrap the original one</param>
        public WrapHandler(
            string exceptionMessage,
            Type wrapExceptionType)
            : base(exceptionMessage, wrapExceptionType)
        {
        }

        /// <summary>
        /// Handles the exception by creating a new one with message and original one as inner exception
        /// </summary>
        /// <param name="exception">The exception to  be handled</param>
        /// <param name="exceptionMessage">The error message associated to the new exception</param>
        /// <returns>A new instance of exception with error message and original one as inner exception</returns>
        protected override Exception HandleException(
            Exception exception,
            string exceptionMessage)
        {
            return CreateExceptionInstance(exceptionMessage, exception);
        }
    }
}
