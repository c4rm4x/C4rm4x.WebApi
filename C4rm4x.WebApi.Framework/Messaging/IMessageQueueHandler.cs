#region Using

using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Messaging
{
    /// <summary>
    /// Service responsible to handler messaging between modules
    /// </summary>
    public interface IMessageQueueHandler
    {
        /// <summary>
        /// Sends a new item into the message queue
        /// </summary>
        /// <param name="item">The new item</param>
        /// <returns>The task</returns>
        Task SendAsync<TItem>(TItem item)
            where TItem : class;
    }
}
