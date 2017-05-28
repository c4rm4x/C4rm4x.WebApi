#region Using

using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Events
{
    /// <summary>
    /// An Event Aggregator acts as a single source of events for many objects. 
    /// It registers for all the events of the many objects allowing clients to register 
    /// with just the aggregator
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        /// Queues up an event of type TEvent with the specfied data but does not broadcast it.
        /// This event only will be broadcasted when PublishAll is run
        /// </summary>
        /// <typeparam name="TEvent">Type of event</typeparam>
        /// <param name="eventData">The event data</param>
        void Enqueue<TEvent>(TEvent eventData)
            where TEvent : ApiEventData;

        /// <summary>
        /// Publishes an event of type TEvent (does not queue it up) with the specified data
        /// </summary>
        /// <typeparam name="TEvent">Type of the event</typeparam>
        /// <param name="eventData">The event data</param>
        /// <returns>The task</returns>
        Task PublishAsync<TEvent>(TEvent eventData)
            where TEvent : ApiEventData;

        /// <summary>
        /// Publishs all the event of type TEvent ready to be broadcasted (previously queued up)
        /// </summary>
        /// <typeparam name="TEvent">Type of event</typeparam>
        /// <returns>The task</returns>
        Task PublishAllAsync<TEvent>()
            where TEvent : ApiEventData;

        /// <summary>
        /// Publishes all the events ready to be broadcasted (previously queued up)
        /// </summary>
        /// <returns>The task</returns>
        Task PublishAllAsync();
    }
}
