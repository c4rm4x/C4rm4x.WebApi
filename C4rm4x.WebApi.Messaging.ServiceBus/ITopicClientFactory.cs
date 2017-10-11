#region Using

using Microsoft.ServiceBus.Messaging;

#endregion

namespace C4rm4x.WebApi.Messaging.ServiceBus
{
    /// <summary>
    /// Service responsible to generate TopicClient instances based on given item
    /// </summary>
    public interface ITopicClientFactory
    {
        /// <summary>
        /// Gets the instance of TopicClient to store the given item
        /// </summary>
        /// <typeparam name="TItem">Type of the item to store</typeparam>
        /// <param name="item">The item to store</param>
        /// <returns>The instance of TopicClient to store the given item</returns>
        TopicClient Get<TItem>(TItem item) where TItem : class;
    }
}
