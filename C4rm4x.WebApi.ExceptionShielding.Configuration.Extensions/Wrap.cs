#region Using

using C4rm4x.WebApi.ExceptionShielding.Handlers;
using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration
{
    /// <summary>
    /// Abstraction of WrapHandler
    /// </summary>
    public static class Wrap
    {
        /// <summary>
        /// Create a new instance of IExceptionHandlerData for a WrapHandler
        /// </summary>
        /// <typeparam name="TException">Type of exception to be wrapped with</typeparam>
        /// <param name="exceptionMessage">Inner messsage</param>
        /// <returns>A new instance of IExceptionHandlerData for a WrapHandler</returns>
        public static IExceptionHandlerData With<TException>(
            string exceptionMessage)
            where TException : Exception
        {
            return With(typeof(TException), exceptionMessage);
        }

        /// <summary>
        /// Create a new instance of IExceptionHandlerData for a WrapHandler
        /// </summary>
        /// <param name="exceptionType">Type to exception to be wrapped with</param>
        /// <param name="exceptionMessage">Inner message</param>
        /// <returns>A new instance of IExceptionHandlerData for a WrapHandler</returns>
        public static IExceptionHandlerData With(
            Type exceptionType,
            string exceptionMessage)
        {
            return new ExceptionHandlerData<WrapHandler>(
                exceptionType, exceptionMessage);
        }
    }
}
