#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.ExceptionShielding.Handlers;
using C4rm4x.WebApi.Framework.Log;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions
{
    /// <summary>
    /// Abstraction of LogHandler
    /// </summary>
    public class Log : IExceptionHandlerData
    {
        /// <summary>
        /// Gets the error message format used to logged the exception
        /// </summary>
        public string ErrorMessageFormat { get; private set; }

        /// <summary>
        /// Gets the severity of how the message will be logged
        /// </summary>
        public Severity Severity { get; private set; }

        /// <summary>
        /// Gets the instance of the logger
        /// </summary>
        public ILog Logger { get; private set; }

        private Log()
        {
            Severity = Severity.Error;
        }

        /// <summary>
        /// Creates a new instance of Log with Severity as Severity.Error
        /// </summary>
        /// <remarks>Entry point to start configuring a new log</remarks>
        /// <param name="errorMessageFormat">Error message format that will be used to log the exception</param>
        /// <returns>A new isntance of Log with Severity as Severity.Error</returns>
        public static Log WithFormat(
            string errorMessageFormat)
        {
            errorMessageFormat.NotNullOrEmpty(nameof(errorMessageFormat));

            return new Log
            {
                ErrorMessageFormat = errorMessageFormat,
            };
        }

        /// <summary>
        /// Sets the severity to log the exception with that level in the log file
        /// </summary>
        /// <param name="severity">The severity</param>
        /// <returns>This log</returns>
        public Log As(Severity severity)
        {
            Severity = severity;

            return this;
        }

        /// <summary>
        /// Sets the logger to be used to log the exception
        /// </summary>
        /// <param name="logger">The logger to be used</param>
        /// <returns>This log</returns>
        public Log Using(ILog logger)
        {
            logger.NotNull(nameof(logger));

            Logger = logger;

            return this;
        }

        /// <summary>
        /// Creates a new instance of IExceptionHandler with the configuration specified
        /// </summary>
        /// <returns>A new instance of IExceptionHandler as configured</returns>
        public IExceptionHandler GetExceptionHandler()
        {
            return new LogHandler(Severity, ErrorMessageFormat, Logger);
        }
    }
}
