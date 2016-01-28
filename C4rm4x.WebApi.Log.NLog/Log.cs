#region Using

using C4rm4x.WebApi.Framework;
using C4rm4x.WebApi.Framework.Log;
using NLog;
using System;

#endregion

namespace C4rm4x.WebApi.Log.NLog
{
    /// <summary>
    /// Implementation of ILog using NLog.Logger
    /// </summary>
    [DomainService(typeof(ILog))]
    public class Log : ILog
    {
        private static readonly Logger _logger =
            LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets the NLog.Logger instance responsible for this log
        /// </summary>
        protected static Logger Logger
        {
            get { return _logger; }
        }

        /// <summary>
        /// Logs a message as Debug
        /// </summary>
        /// <param name="message">The message</param>
        public void Debug(string message)
        {
            Logger.Debug(message);
        }

        /// <summary>
        /// Logs a message as Info
        /// </summary>
        /// <param name="message">The message</param>
        public void Info(string message)
        {
            Logger.Info(message);
        }

        /// <summary>
        /// Logs a message as Error
        /// </summary>
        /// <param name="message">The message</param>
        public void Error(string message)
        {
            Logger.Error(message);
        }

        /// <summary>
        /// Logs a message as Error and adds exception trace
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="exception">The exception</param>
        public void Error(
            string message,
            Exception exception)
        {
            Logger.Error(exception, message);
        }
    }
}
