using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Events.EF.Infrastructure;
using C4rm4x.WebApi.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.Entity;

namespace C4rm4x.WebApi.Events.EF.Test
{
    public partial class EventStoreTest
    {
        [TestClass]
        public abstract class EventStoreFixture :
            AutoMockFixture<EventStore>
        {
            #region Helper classes            

            public class SensitiveApiEventData : ApiEventData
            {
                public SensitiveApiEventData() : base()
                {
                }
            }

            public class RegularApiEventData : ApiEventData
            {
                public string Property { get; private set; }

                public RegularApiEventData(string property) : base()
                {
                    Property = property;
                }
            }

            #endregion

            [TestInitialize]
            public override void Setup()
            {
                base.Setup();

                GetMock<EventStoreContext>()
                    .SetupGet(c => c.Events)
                    .Returns(Mock.Of<DbSet<Event>>());

                EventStorePolicy.SetConfiguration(GetConfiguration());
            }

            private static IEventStoreConfiguration GetConfiguration()
            {
                var configuration = Mock.Of<IEventStoreConfiguration>();

                Mock.Get(configuration)
                    .Setup(c => c.IsSensitive(It.IsAny<Type>()))
                    .Returns(false);

                Mock.Get(configuration)
                    .Setup(c => c.IsSensitive(typeof(SensitiveApiEventData)))
                    .Returns(true);

                return configuration;
            }
        }
    }
}