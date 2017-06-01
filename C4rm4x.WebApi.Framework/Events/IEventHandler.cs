#region Using

using System;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Events
{
    /// <summary>
    /// Interface that handles events
    /// </summary>
    public interface IEventHandler
    {
        /// <summary>
        /// Handles an object
        /// </summary>
        /// <param name="eventData">The event data associated with the event to be handled</param>
        /// <returns>The task</returns>

        Task OnEventHandlerAsync(object eventData);

        /// <summary>
        /// Checks to see whether the handler can handle objects of the specified type
        /// </summary>
        /// <returns>True if handler can handle object of the specified type. False otherwise</returns>
        bool CanHandleInstancesOf(Type type);
    }

    /// <summary>
    /// Interface that handles all events of type TEvent using a instance as payload
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface IEventHandler<TEvent> : IEventHandler
        where TEvent : ApiEventData
    {
        /// <summary>
        /// Handles event of type T with specified data
        /// </summary>
        /// <param name="eventData">The event data associated with the event to be handled</param>
        /// <returns>The task</returns>
        Task OnEventHandlerAsync(TEvent eventData);
    }
}
