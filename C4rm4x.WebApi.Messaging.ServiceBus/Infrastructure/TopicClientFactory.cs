#region Using

using C4rm4x.Tools.Utilities;
using Microsoft.ServiceBus.Messaging;

#endregion

namespace C4rm4x.WebApi.Messaging.ServiceBus.Infrastructure
{
    /// <summary>
    /// Service responsible to generate instances of TopicClient
    /// </summary>
    public class TopicClientFactory
    {
        /// <summary>
        /// Gets the connection string
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        public TopicClientFactory(
            string connectionString)
        {
            connectionString.NotNullOrEmpty(nameof(connectionString));

            ConnectionString = connectionString;
        }

        /// <summary>
        /// Gets an instance of TopicClient based on the given configuration
        /// </summary>
        /// <param name="path">The topic path</param>
        /// <returns>An instance of TopicClient</returns>
        public TopicClient Get(string path)
        {
            return TopicClient
                .CreateFromConnectionString(ConnectionString, path);
        }
    }
}
