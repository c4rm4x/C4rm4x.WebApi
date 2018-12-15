using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework;
using System;

namespace C4rm4x.WebApi.Events.EF
{
    /// <summary>    /// Represents a policy for event store processing    /// </summary>    public class EventStorePolicy    {        /// <summary>        /// Gets the given configuration (if any)        /// </summary>        public static IEventStoreConfiguration Configuration { get; private set; }        /// <summary>        /// Sets the event store configuration        /// </summary>        /// <param name="configuration">The configuration to use</param>        public static void SetConfiguration(            IEventStoreConfiguration configuration)        {            configuration.NotNull(nameof(configuration));
            Configuration = configuration;        }
        /// <summary>        /// Checks whether the given object type has been flagged as sensitive type of event        /// </summary>        /// <typeparam name="T">The type of the event</typeparam>        /// <param name="eventData">The instance of ApiEventData</param>        /// <returns>True when the given object type has been flagged as sensitive; false, otherwise</returns>        public static bool IsSensitive<T>(T eventData) where T : ApiEventData        {            return IsSensitive(eventData.GetType());        }
        /// <summary>        /// Checks whether the given type has been flagged as sensitive type of event        /// </summary>        /// <param name="eventDataType">The event data type</param>        /// <returns>True when the given type has been flagged as sensitive; false, otherwise</returns>        public static bool IsSensitive(Type eventDataType) =>
            Configuration?.IsSensitive(eventDataType) ?? default(bool);
    }
}
