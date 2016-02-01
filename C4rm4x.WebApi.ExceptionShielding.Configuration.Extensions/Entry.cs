#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions
{
    /// <summary>
    /// Abstraction of ExceptionPolicyEntry
    /// </summary>
    public class Entry
    {
        private readonly ICollection<IExceptionHandlerData> _handlers;

        /// <summary>
        /// Gets the exception type to be handled
        /// </summary>
        public Type ExceptionType { get; private set; }

        /// <summary>
        /// Gets the action to do after the exception is handled
        /// </summary>
        public PostHandlingAction Action { get; private set; }

        private Entry(Type exceptionType)
        {
            exceptionType.NotNull(nameof(exceptionType));
            exceptionType.Is<Exception>();

            ExceptionType = exceptionType;
            _handlers = new List<IExceptionHandlerData>();
        }

        /// <summary>
        /// Creates a new instance of Entry
        /// </summary>
        /// <remarks>
        /// This is the entry point to start configuring an entry
        /// </remarks>
        /// <typeparam name="TException">Type of the exception to be handled</typeparam>
        /// <returns>A new instance of Entry</returns>
        public static Entry For<TException>()
            where TException : Exception
        {
            return new Entry(typeof(TException));
        }

        /// <summary>
        /// Sets the action to be performed once the exception has been handled
        /// </summary>
        /// <param name="then">The action</param>
        /// <returns>This entry</returns>
        public Entry Then(PostHandlingAction then)
        {
            Action = then;

            return this;
        }

        /// <summary>
        /// Adds a new exception handler data (abstraction of IExceptionHandler)
        /// within this entry
        /// </summary>
        /// <param name="exceptionHandlerData">The exception handler data</param>
        /// <returns>This entry</returns>
        public Entry WithExceptionHandler(
            IExceptionHandlerData exceptionHandlerData)
        {
            exceptionHandlerData.NotNull(nameof(exceptionHandlerData));

            _handlers.Add(exceptionHandlerData);

            return this;
        }

        /// <summary>
        /// Gets the equivalent ExceptionPolicyEntry for this one
        /// </summary>
        /// <returns>A new ExceptionPolicyEntry instance based on this configuration</returns>		
        public ExceptionPolicyEntry GetExceptionPolicyEntry()
        {
            return new ExceptionPolicyEntry(
                ExceptionType,
                Action,
                _handlers.Select(handler => handler.GetExceptionHandler()));
        }
    }
}
