#region Using

using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Events
{
    /// <summary>
    /// Interface of Event Store responsible to persist all events raised
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Saves all the events
        /// </summary>
        /// <param name="events">The events raised to be saved</param>        
        Task SaveAllAsync(IEnumerable<ApiEventData> events);
    }
}
