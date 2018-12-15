using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework;
using System;

namespace C4rm4x.WebApi.Events.EF.Infrastructure
{
    /// <summary>    /// The event entity    /// </summary>    public class Event    {        /// <summary>        /// Gets the event ID        /// </summary>        public int EventID { get; private set; }        /// <summary>
        /// Gets the aggregate ID
        /// </summary>        public Guid AggregateID { get; private set; }         
        /// <summary>        /// Gets the aggregate version        /// </summary>        public int Version { get; private set; }
        /// <summary>        /// Gets the event timestamp        /// </summary>        public DateTime TimeStamp { get; private set; }
        /// <summary>        /// Gets the event type        /// </summary>        public string Type { get; private set; }
        /// <summary>        /// Gets the event payload (if any)        /// </summary>        public string Payload { get; private set; }
        private Event()        {
        }
        internal static Event Create(            ApiEventData eventData, string payload = null)        {            eventData.NotNull(nameof(eventData));
            return new Event            {                AggregateID = eventData.Id,                Version = eventData.Version,                TimeStamp = eventData.TimeStamp,                Type = eventData.GetType().FullName,                Payload = payload,            };        }    }
}
