#region Using

using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Events
{
    /// <summary>
    /// Abstract class that implements IEventPublisher
    /// </summary>
    public abstract class AbstractEventPublisher : IEventPublisher
    {
        /// <summary>
        /// Publish event ready to be processed
        /// </summary>
        /// <typeparam name="TEvent">Type of event</typeparam>
        /// <param name="eventData">The event payload</param>
        /// <returns></returns>
        public Task PublishAsync<TEvent>(TEvent eventData) 
            where TEvent : ApiEventData
        {
            var handlers = GetHandlers(eventData.GetType()).Cast<IEventHandler<TEvent>>();

            return Task.WhenAll(handlers.Select(
                handler => handler.OnEventHandlerAsync(eventData)));
        }

        /// <summary>
        /// Get all the registered handlers interested in events of the given type
        /// </summary>
        /// <param name="eventDataType">The actual type of the event</param>
        /// <returns></returns>
        protected abstract IEnumerable GetHandlers(Type eventDataType);
    }
}
