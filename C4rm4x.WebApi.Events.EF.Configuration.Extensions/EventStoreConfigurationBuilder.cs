using C4rm4x.WebApi.Framework;

namespace C4rm4x.WebApi.Events.EF.Configuration
{
    /// <summary>    /// Configures the EventStoreConfiguration using a fluid interface    /// </summary>    public class EventStoreConfigurationBuilder    {        private readonly EventStoreConfiguration _configuration;

        private EventStoreConfigurationBuilder()        {            _configuration = EventStoreConfiguration.Create();        }
        /// <summary>        /// Creates a new instance of EventStoreConfigurationBuilder        /// </summary>        /// <remarks>        /// This is the entry point to start configuring a new EventStoreConfiguration        /// </remarks>        /// <returns>A new instance of EventStoreConfigurationBuilder</returns>        public static EventStoreConfigurationBuilder Configure() => new EventStoreConfigurationBuilder();
        /// <summary>        /// Flags events of type T as sensitive        /// </summary>        /// <typeparam name="T">Type of the event to be flagged as sensitive</typeparam>        /// <returns>This builder</returns>        public EventStoreConfigurationBuilder SensitivePayload<T>() where T : ApiEventData        {            _configuration.SensitivePayload<T>();            return this;        }
        /// <summary>        /// Creates a new instance of EventStoreConfiguration with the configuration specified at this moment        /// </summary>        /// <returns>A new instance of EventStoreConfiguration configured with the specified rules</returns>        public IEventStoreConfiguration Build() => _configuration;    }
}
