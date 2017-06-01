#region Using

using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Events
{
    /// <summary>
    /// Service responsible to publish events to be processed for registered handlers
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publish event ready to be processed
        /// </summary>
        /// <typeparam name="TEvent">Type of event</typeparam>
        /// <param name="eventData">The event payload</param>
        /// <returns></returns>
        Task PublishAsync<TEvent>(TEvent eventData)
            where TEvent : ApiEventData;
    }
}
