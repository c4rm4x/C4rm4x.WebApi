#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Events
{
    /// <summary>
    /// Base implementation of IEventHandler
    /// </summary>
    public abstract class AbstractEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : ApiEventData
    {
        /// <summary>
        /// Checks to see whether the handler can handle objects of the specified type
        /// </summary>
        /// <returns>True if handler can handle object of the specified type. False otherwise</returns>
        public bool CanHandleInstancesOf(Type type)
        {
            return typeof(TEvent).IsAssignableFrom(type);
        }

        /// <summary>
        /// Handles an object
        /// </summary>
        /// <param name="eventData">The event data associated with the event to be handled</param>
        /// <returns>The task</returns>
        public Task OnEventHandlerAsync(object eventData)
        {
            eventData.NotNull(nameof(eventData));

            if (!this.CanBeHandled(eventData))
                throw new ArgumentException(
                    string.Format("And object of type {0} cannot be handled against this handler {1}",
                    eventData.GetType().FullName, this.GetType().FullName));

            return OnEventHandlerAsync((TEvent)eventData);
        }

        private bool CanBeHandled(object objectToHandle)
        {
            return CanHandleInstancesOf(objectToHandle.GetType());
        }

        /// <summary>
        /// Handles event of type T with specified data
        /// </summary>
        /// <param name="eventData">The event data associated with the event to be handled</param>
        /// <returns>The task</returns>
        public abstract Task OnEventHandlerAsync(TEvent eventData);
    }
}
