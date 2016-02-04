#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Log;
using System;
using System.Collections;
using System.Text;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Handlers
{
    #region Enums

    /// <summary>
    /// Defines the minimum set of levels recognized by the system
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// This is the most verbose logging level (maximum volume setting)
        /// </summary>
        Debug,

        /// <summary>
        /// The Information level is typically used to output information that is useful to 
        /// the running and management of your system. 
        /// Information would also be the level used to log Entry and Exit points in key 
        /// areas of your application
        /// </summary>
        Info,

        /// <summary>
        /// Error is used to log all unhandled exceptions. 
        /// This is typically logged inside a catch block at the boundary of your application
        /// </summary>
        Error
    }

    #endregion

    /// <summary>
    /// Represents an IExceptionHandler that formats
    /// the exception into a log message and sends it to
    /// the ILog specified as parameter
    /// </summary>
    public class LogHandler : IExceptionHandler
    {
        private readonly ILog _logger;

        /// <summary>
        /// Gets the severity used to log the error messages 
        /// </summary>
        public Severity Severity { get; private set; }

        /// <summary>
        /// Gets the message format used to log the exceptions
        /// </summary>
        public string ErrorMessageFormat { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="severity">Severity used to log the error messages</param>
        /// <param name="errorMessageFormat">The message format used to log the exceptions</param>
        /// <param name="logger">The logger</param>
        public LogHandler(
            Severity severity,
            string errorMessageFormat,
            ILog logger)
        {
            logger.NotNull(nameof(logger));
            errorMessageFormat.NotNullOrEmpty(nameof(errorMessageFormat));

            Severity = severity;
            ErrorMessageFormat = errorMessageFormat;
            _logger = logger;
        }

        /// <summary>
        /// Handles the exception by writing in the log
        /// </summary>
        /// <param name="exception">The exception to be handled</param>        
        /// <param name="handlingInstanceId">The unique ID attached to the handling chain for this handling instance</param>
        /// <returns>Modified exception to pass to the next exceptionHandler in the chain</returns>
        public Exception HandleException(
            Exception exception,
            Guid handlingInstanceId)
        {
            WriteLog(exception, handlingInstanceId);

            return exception;
        }

        private void WriteLog(
            Exception exception,
            Guid handlingInstanceId)
        {
            var message = CreateMessage(exception, handlingInstanceId);

            switch (Severity)
            {
                case Severity.Error:
                    _logger.Error(message);
                    break;

                case Severity.Info:
                    _logger.Info(message);
                    break;

                case Severity.Debug:
                    _logger.Debug(message);
                    break;
            }
        }

        private string CreateMessage(
            Exception exception,
            Guid handlingInstanceId)
        {
            var message = new StringBuilder();

            message.AppendLine(FormatUtilities
                .FormatException(ErrorMessageFormat, handlingInstanceId));
            message.AppendLine("----------------------------");

            message.AppendLine(exception.Message);
            message.AppendLine("----------------------------");

            message.AppendLine(exception.StackTrace);
            message.AppendLine("----------------------------");

            foreach (DictionaryEntry dataEntry in exception.Data)
                if (dataEntry.Key is string)
                    message.AppendLine(
                        "\t{0}: {1}".AsFormat(
                            dataEntry.Key as string,
                            dataEntry.Value));

            return message.ToString();
        }
    }
}
