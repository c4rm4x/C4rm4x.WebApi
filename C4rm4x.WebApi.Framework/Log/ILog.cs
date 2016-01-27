#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Framework.Log
{
    /// <summary>
    /// Service to log information in a log file
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Logs a message as Debug
        /// </summary>
        /// <param name="message">The message</param>
        void Debug(string message);

        /// <summary>
        /// Logs a message as Info
        /// </summary>
        /// <param name="message">The message</param>
        void Info(string message);

        /// <summary>
        /// Logs a message as Error
        /// </summary>
        /// <param name="message">The message</param>
        void Error(string message);


        /// <summary>
        /// Logs a message as Error and adds exception trace
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="exception">The exception</param>
        void Error(
            string message,
            Exception exception);
    }
}
