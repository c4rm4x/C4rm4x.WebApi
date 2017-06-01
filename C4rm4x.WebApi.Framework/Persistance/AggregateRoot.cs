#region Using

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Framework
{
    /// <summary>
    /// Aggregate root (DDD)
    /// </summary>
    public abstract class AggregateRoot
    {        
        private ICollection<ApiEventData> _events;

        public Guid Id { get; private set; }

        public int Version { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        protected AggregateRoot()
        {
            Id = Guid.NewGuid();
            Version = 0;

            _events = new List<ApiEventData>();
        }

        /// <summary>
        /// Apply event
        /// </summary>
        /// <param name="event">The event to be applied</param>
        protected void ApplyEvent(ApiEventData @event)
        {
            _events.Add(@event);
        }

        /// <summary>
        /// Pop all uncommited events
        /// </summary>
        /// <returns>The collection of events pending to be processed</returns>
        public IEnumerable<ApiEventData> FlushEvents()
        {
            var events = _events.ToArray();

            foreach (var @event in events)
            {
                @event.Id = Id;
                @event.Version = ++Version;
            }

            _events.Clear();

            return events;
        }
    }
}
