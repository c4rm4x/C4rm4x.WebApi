#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding
{
    #region Interface

    /// <summary>
    /// Represents an entry in an ExceptionPolicy containing
    /// an exception type as the key and a list of 
    /// IExceptionHandler instances as the value.
    /// </summary>
    public interface IExceptionPolicyEntry
    {
        /// <summary>
        /// Type of exception to handle
        /// </summary>
        Type ExceptionType { get; }

        /// <summary>
        /// Handles all exceptions in the chain
        /// </summary>
        /// <param name="exceptionToHandle">The exception to handle</param>
        /// <returns>Whether or not a rethrow is recommended</returns>
        bool Handle(Exception exceptionToHandle);
    }

    #endregion

    /// <summary>
    /// Represents an entry in an ExceptionPolicy containing
    /// an exception type as the key and a list of 
    /// IExceptionHandler instances as the value.
    /// </summary>
    public class ExceptionPolicyEntry : IExceptionPolicyEntry
    {
        private readonly IEnumerable<IExceptionHandler> _handlers;

        /// <summary>
        /// Gets the type of exception to handle
        /// </summary>
        public Type ExceptionType { get; private set; }

        /// <summary>
        /// Gets the action to do after the exception is handled.
        /// </summary>
        public PostHandlingAction Action { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exceptionType">Type of exception this policy refers to</param>
        /// <param name="action">What to do after the exception is handled</param>
        /// <param name="handlers">Handlers to execute on the exception</param>
        public ExceptionPolicyEntry(
            Type exceptionType,
            PostHandlingAction action,
            IEnumerable<IExceptionHandler> handlers)
        {
            exceptionType.NotNull(nameof(exceptionType));
            handlers.NotNullOrEmpty(nameof(handlers));

            ExceptionType = exceptionType;
            Action = action;
            _handlers = handlers.ToArray();
        }

        /// <summary>
        /// Handles all exceptions in the chain
        /// </summary>
        /// <param name="exceptionToHandle">The exception to handle</param>
        /// <returns>Whether or not a rethrow is recommended</returns>
        public bool Handle(Exception exceptionToHandle)
        {
            exceptionToHandle.NotNull(nameof(exceptionToHandle));

            return RethrowRecommended(
                ExecuteHandlerChain(exceptionToHandle, Guid.NewGuid()));
        }

        private bool RethrowRecommended(Exception exceptionToThrow)
        {
            if (Action == PostHandlingAction.ThrowNewException)
                throw InternalRethrow(exceptionToThrow);

            return Action == PostHandlingAction.ShouldRethrow;
        }

        private static Exception InternalRethrow(
            Exception exceptionToThrow)
        {
            if (exceptionToThrow != null)
                throw exceptionToThrow;

            return new ArgumentNullException(nameof(exceptionToThrow));
        }

        private Exception ExecuteHandlerChain(
            Exception ex,
            Guid handlingInstanceID)
        {
            var lastHandlerName = string.Empty;

            try
            {
                // Iterates through all the handlers and returns last exception
                foreach (var handler in _handlers)
                {
                    lastHandlerName = handler.GetType().Name;
                    ex = handler.HandleException(ex, handlingInstanceID);
                }
            }
            catch (Exception handlingException)
            {
                throw new InvalidOperationException(
                    "Unable to handle exception {0}".AsFormat(lastHandlerName),
                    handlingException);
            }

            return ex;
        }
    }
}
