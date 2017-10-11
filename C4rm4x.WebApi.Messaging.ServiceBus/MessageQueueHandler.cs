#region Using

using C4rm4x.Tools.ServiceBus;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Messaging;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Messaging.ServiceBus
{
    /// <summary>
    /// Implementation of IMessageQueueHandler using ServiceBus TopicClient
    /// </summary>
    public class MessageQueueHandler : IMessageQueueHandler
    {
        private readonly ITopicClientFactory _factory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="factory">The topic client factory</param>
        public MessageQueueHandler(ITopicClientFactory factory)
        {
            factory.NotNull(nameof(factory));

            _factory = factory;
        }

        /// <summary>
        /// Sends a new item into the message queue
        /// </summary>
        /// <typeparam name="TItem">Type of the item</typeparam>
        /// <param name="item">The new item</param>
        public Task SendAsync<TItem>(TItem item) 
            where TItem : class
        {
            return _factory.Get(item).SendAsync(item.BuildBrokeredMessage());
        }
    }
}