#region Using

using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Events
{
    /// <summary>
    /// Default event store that saves nothing
    /// </summary>
    public class NoEventStore : IEventStore
    {
        /// <summary>
        /// Saves all the events
        /// </summary>
        /// <param name="events">The events raised to be saved</param>        
        public Task SaveAllAsync(IEnumerable<ApiEventData> events)
        {
            return Task.FromResult(true);
        }
    }
}
