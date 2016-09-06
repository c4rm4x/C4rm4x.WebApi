#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Messaging;
using Microsoft.ServiceBus.Messaging;
using System;

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
        public void Send<TItem>(TItem item) 
            where TItem : class
        {
            _sender.Send(GetBrokeredMessage(item));
        }

        private static BrokeredMessage GetBrokeredMessage<TItem>(TItem item) 
            where TItem : class
        {
            item.NotNull(nameof(item));

            var brokeredMessage = new BrokeredMessage(item);

            brokeredMessage.Label = "{0}_{1}".AsFormat(typeof(TItem).Name, DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
            brokeredMessage.MessageId = Guid.NewGuid().ToString();
            brokeredMessage.ContentType = typeof(TItem).AssemblyQualifiedName;

            return brokeredMessage;
        }
    }
}