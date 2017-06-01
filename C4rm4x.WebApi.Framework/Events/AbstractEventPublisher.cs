#region Using

using System;
using System.Collections.Generic;
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
        public async Task PublishAsync<TEvent>(TEvent eventData) 
            where TEvent : ApiEventData
        {
            var handlers = GetHandlers(
                typeof(IEventHandler<>).MakeGenericType(eventData.GetType()));

            await Task.WhenAll(handlers.Select(
                handler => handler.OnEventHandlerAsync(eventData)));
        }

        /// <summary>
        /// Get all the registered handlers for the given type
        /// </summary>
        /// <param name="type">The type of events</param>
        protected abstract IEnumerable<IEventHandler> GetHandlers(Type type);
    }
}
