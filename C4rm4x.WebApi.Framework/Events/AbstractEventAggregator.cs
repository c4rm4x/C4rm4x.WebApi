#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Framework.Events
{
    /// <summary>
    /// Base implementation of IEventAggregator
    /// </summary>
    public abstract class AbstractEventAggregator
        : IEventAggregator
    {
        /// <summary>
        /// Gets the queue with all the event data elements pending to be publish
        /// </summary>
        protected static ConcurrentQueue<ApiEventData> Queue { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AbstractEventAggregator()
        {
            Queue = new ConcurrentQueue<ApiEventData>();
        }

        /// <summary>
        /// Queues up an event of type TEvent with the specfied data but does not broadcast it.
        /// This event only will be broadcasted when PublishAll is run
        /// </summary>
        /// <typeparam name="TEvent">Type of event</typeparam>
        /// <param name="eventData">The event data</param>
        public void Enqueue<TEvent>(TEvent eventData)
            where TEvent : ApiEventData
        {
            eventData.NotNull(nameof(eventData));

            Queue.Enqueue(eventData);
        }

        /// <summary>
        /// Publishes an event of type TEvent (does not queue it up) with the specified data
        /// </summary>
        /// <typeparam name="TEvent">Type of the event</typeparam>
        /// <param name="eventData">The event data</param>
        public void Publish<TEvent>(TEvent eventData)
            where TEvent : ApiEventData
        {
            eventData.NotNull(nameof(eventData));

            foreach (var handler in GetHandlers<TEvent>())
                handler.OnEventHandler(eventData);
        }

        /// <summary>
        /// Retrieves the list of all the event handlers that implement the interface
        /// IEventHandler for the specified type TEvent
        /// </summary>
        /// <typeparam name="TEvent">Type of the event</typeparam>
        /// <returns>The list of all (if any) the event handlers for the specified type of event</returns>
        protected abstract IEnumerable<IEventHandler<TEvent>> GetHandlers<TEvent>()
            where TEvent : ApiEventData;

        /// <summary>
        /// Publishs all the event of type TEvent ready to be broadcasted (previously queued up)
        /// </summary>
        /// <typeparam name="TEvent">Type of event</typeparam>
        public void PublishAll<TEvent>()
            where TEvent : ApiEventData
        {
            PublishAll(Queue.OfType<TEvent>());
            CleanQueue(e => e.GetType() == typeof(TEvent));
        }

        private void PublishAll<TEvent>(
            IEnumerable<TEvent> eventDatas)
            where TEvent : ApiEventData
        {
            foreach (var eventData in eventDatas)
                Publish(eventData);
        }

        private static void CleanQueue(
            Func<ApiEventData, bool> removeIf)
        {
            Queue = new ConcurrentQueue<ApiEventData>(
                Queue.Where(item => !removeIf(item)));
        }

        /// <summary>
        /// Publishes all the events ready to be broadcasted (previously queued up)
        /// </summary>
        public void PublishAll()
        {
            throw new NotImplementedException();
        }
    }
}
