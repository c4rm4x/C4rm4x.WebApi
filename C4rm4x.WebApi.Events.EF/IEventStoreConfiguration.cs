using C4rm4x.WebApi.Framework;
using System;
using System.Collections.Generic;

namespace C4rm4x.WebApi.Events.EF
{
    /// <summary>
    /// Non-static entry point to the event storage functionality.
    /// </summary>
    public interface IEventStoreConfiguration
    {
        /// <summary>
        /// Checks whether the given object type has been flagged as sensitive type of event
        /// </summary>
        /// <typeparam name="T">The type of the event</typeparam>
        /// <param name="eventData">The instance of ApiEventData</param>
        /// <returns>True when the given object type has been flagged as sensitive; false, otherwise</returns>
        bool IsSensitive<T>(T eventData) where T : ApiEventData;

        /// <summary>
        /// Checks whether the given type has been flagged as sensitive type of event
        /// </summary>
        /// <param name="eventDataType">The event data type</param>
        /// <returns>True when the given type has been flagged as sensitive; false, otherwise</returns>
        bool IsSensitive(Type eventDataType);

        /// <summary>
        /// Returns the collection of types to be ignored during serialization (if any)
        /// </summary>
        IEnumerable<Type> TypesToIgnore { get; }
    }
}