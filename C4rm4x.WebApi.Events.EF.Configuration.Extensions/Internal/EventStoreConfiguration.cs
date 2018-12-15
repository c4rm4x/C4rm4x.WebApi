using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework;
using System;
using System.Collections.Generic;

namespace C4rm4x.WebApi.Events.EF.Configuration
{
    internal class EventStoreConfiguration : IEventStoreConfiguration    {        public ICollection<Type> SensitiveEvents { get; private set; }
        private EventStoreConfiguration()        {
            SensitiveEvents = new HashSet<Type>();
        }
        public static EventStoreConfiguration Create() => new EventStoreConfiguration();
        public bool IsSensitive<T>(T eventData) where T : ApiEventData        {            return IsSensitive(eventData.GetType());        }
        public bool IsSensitive(Type eventDataType)        {            eventDataType.Is<ApiEventData>();
            return SensitiveEvents.Contains(eventDataType);        }
        internal EventStoreConfiguration SensitivePayload<T>() where T : ApiEventData        {            return SensitivePayload(typeof(T));        }
        internal EventStoreConfiguration SensitivePayload(Type eventDataType)        {            eventDataType.Is<ApiEventData>();            SensitiveEvents.Add(eventDataType);
            return this;        }    }
}
