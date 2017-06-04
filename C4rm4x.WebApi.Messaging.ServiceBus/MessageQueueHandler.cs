#region Using

using C4rm4x.Tools.ServiceBus;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Messaging;
using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Messaging.ServiceBus
{
    /// <summary>
    /// Implementation of IMessageQueueHandler using ServiceBus TopicClient
    /// </summary>
    public class MessageQueueHandler : IMessageQueueHandler
    {
        private readonly TopicClient _sender;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender">The instance of IMessageSender used to send messages through</param>
        public MessageQueueHandler(
            TopicClient sender)
        {
            sender.NotNull(nameof(sender));

            _sender = sender;
        }

        /// <summary>
        /// Sends a new item into the message queue
        /// </summary>
        /// <typeparam name="TItem">Type of the item</typeparam>
        /// <param name="item">The new item</param>
        public Task SendAsync<TItem>(TItem item) 
            where TItem : class
        {
            return _sender.SendAsync(item.BuildBrokeredMessage());
        }
    }
}