#region Using

using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Events
{
    /// <summary>
    /// Interface that handles all events of type TEvent using a instance as payload
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface IEventHandler<TEvent>
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
