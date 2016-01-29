#region Using

using C4rm4x.Tools.Utilities;
using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Handlers
{
    /// <summary>
    /// Base implementation of IExceptionHandler which creates a new exception based on 
    /// specified type
    /// </summary>
    public abstract class AbstractHandler : IExceptionHandler
    {
        private readonly string _exceptionMessage;

        /// <summary>
        /// Gets the type of the exception to be returned
        /// </summary>
        protected Type ExceptionType { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exceptionMessage">Exception message to be included in the new exception</param>
        /// <param name="exceptionType">Type of the new exception to be created</param>
        public AbstractHandler(
            string exceptionMessage,
            Type exceptionType)
        {
            exceptionMessage.NotNullOrEmpty(nameof(exceptionMessage));
            exceptionType.NotNull(nameof(exceptionType));
            exceptionType.Is<Exception>();

            _exceptionMessage = exceptionMessage;
            ExceptionType = exceptionType;
        }

        /// <summary>
        /// Handles the exception
        /// </summary>
        /// <param name="exception">The exception to be handled</param>        
        /// <param name="handlingInstanceId">The unique ID attached to the handling chain for this handling instance</param>
        /// <returns>Modified exception to pass to the next exceptionHandler in the chain</returns>
        public Exception HandleException(
            Exception exception,
            Guid handlingInstanceId)
        {
            return HandleException(
                exception,
                FormatUtilities
                    .FormatException(_exceptionMessage, handlingInstanceId));
        }

        /// <summary>
        /// Creates a new instance of the exception to be created using the optional args
        /// </summary>
        /// <param name="args">Optional args to create the new instance</param>
        /// <returns>A new instance of the excepiton of type ExceptionType</returns>
        protected Exception CreateExceptionInstance(params object[] args)
        {
            return (Exception)Activator.CreateInstance(ExceptionType, args);
        }

        /// <summary>
        /// Handles the exception based on specific implementations rules
        /// </summary>
        /// <param name="exception">The exception to be handled</param>
        /// <param name="exceptionMessage">The new error message associated to the new exception</param>
        /// <returns></returns>
        protected abstract Exception HandleException(
            Exception exception,
            string exceptionMessage);
    }
}
