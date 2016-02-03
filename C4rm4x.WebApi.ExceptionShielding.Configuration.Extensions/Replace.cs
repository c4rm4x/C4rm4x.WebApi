#region Using

using C4rm4x.WebApi.ExceptionShielding.Handlers;
using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration
{
    /// <summary>
    /// Abstraction of ReplaceHandler
    /// </summary>
    public static class Replace
    {
        /// <summary>
        /// Create a new instance of IExceptionHandlerData for a ReplaceHandler
        /// </summary>
        /// <typeparam name="TException">Type of exception to be replaced by</typeparam>
        /// <param name="exceptionMessage">Inner messsage</param>
        /// <returns>A new instance of IExceptionHandlerData for a ReplaceHandler</returns>
        public static IExceptionHandlerData By<TException>(
            string exceptionMessage)
            where TException : Exception
        {
            return By(typeof(TException), exceptionMessage);
        }

        /// <summary>
        /// Create a new instance of IExceptionHandlerData for a ReplaceHandler
        /// </summary>
        /// <param name="exceptionType">Type to exception to be replaced by</param>
        /// <param name="exceptionMessage">Inner message</param>
        /// <returns>A new instance of IExceptionHandlerData for a ReplaceHandler</returns>
        public static IExceptionHandlerData By(
            Type exceptionType,
            string exceptionMessage)
        {
            return new ExceptionHandlerData<ReplaceHandler>(
                exceptionType, exceptionMessage);
        }
    }
}
