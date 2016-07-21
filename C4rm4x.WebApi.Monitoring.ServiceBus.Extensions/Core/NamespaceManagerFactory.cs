#region Using

using C4rm4x.Tools.Utilities;
using Microsoft.ServiceBus;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceBus.Core
{
    #region Interface

    /// <summary>
    /// Service responsible to return the instance of NamespaceManager based on application configuration
    /// </summary>
    public interface INamespaceManagerFactory
    {
        /// <summary>
        /// Returns an instance of NamespaceManager based on configuration
        /// </summary>
        /// <returns>An instance of NamespaceManager</returns>
        NamespaceManager Get();
    }

    #endregion

    /// <summary>
    /// Implementation of INamespaceManagerFactory
    /// </summary>
    public class NamespaceManagerFactory : INamespaceManagerFactory
    {
        /// <summary>
        /// Gets the connection string
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        public NamespaceManagerFactory(string connectionString)
        {
            connectionString.NotNullOrEmpty(nameof(connectionString));

            ConnectionString = connectionString;
        }

        /// <summary>
        /// Returns an instance of NamespaceManager based on configuration
        /// </summary>
        /// <returns>An instance of NamespaceManager</returns>
        public NamespaceManager Get()
        {
            return NamespaceManager.CreateFromConnectionString(ConnectionString);
        }
    }
}
