#region Using

using C4rm4x.Tools.Utilities;
using Microsoft.ServiceBus.Messaging;

#endregion

namespace C4rm4x.WebApi.Messaging.ServiceBus
{
    /// <summary>
    /// Service that returns always the same instance of TopicClient
    /// </summary>
    public class SimpleTopicClientFactory : ITopicClientFactory
    {
        /// <summary>
        /// Gets the instance of TopicClient
        /// </summary>
        public TopicClient Client { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">The instance of TopicClient</param>
        public SimpleTopicClientFactory(TopicClient client)
        {
            client.NotNull(nameof(client));

            Client = client;
        }

        /// <summary>
        /// Gets the instance of TopicClient to store the given item
        /// </summary>
        /// <typeparam name="TItem">Type of the item to store</typeparam>
        /// <param name="item">The item to store</param>
        /// <returns>The instance of TopicClient to store the given item</returns>
        /// <remarks>Returns always the same instance regardless the item to store</remarks>
        public TopicClient Get<TItem>(TItem item) where TItem : class => Client;
    }
}
