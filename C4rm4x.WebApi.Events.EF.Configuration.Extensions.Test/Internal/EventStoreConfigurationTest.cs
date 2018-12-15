using C4rm4x.WebApi.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace C4rm4x.WebApi.Events.EF.Configuration.Test
{
    public partial class EventStoreConfigurationTest
    {
        [TestClass]
        public abstract class EventStoreConfigurationFixture
        {
            protected IEventStoreConfiguration _sut = EventStoreConfiguration.Create();

            protected IEnumerable<Type> TryAddSensitivePayload<T>()
                where T : ApiEventData
            {
                var configuration = _sut as EventStoreConfiguration;

                configuration.SensitivePayload<T>();

                return configuration.SensitiveEvents;
            }

            #region Helper classes

            public class TestApiEventData : ApiEventData
            {
                public TestApiEventData() : base()
                {
                }
            }

            #endregion
        }
    }
}