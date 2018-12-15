using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.TestUtilities.Builders;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Events.EF.Infrastructure;
using C4rm4x.WebApi.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace C4rm4x.WebApi.Events.EF.Test
{
    public partial class EventStoreTest
    {
        [TestClass]
        public class EventStoreSaveAllAsyncTest : EventStoreFixture
        {
            [TestMethod, UnitTest]

            public async Task SaveAllAsync_Uses_SaveChangesAsync_From_EventStoreContext()
            {
                await _sut.SaveAllAsync(new ApiEventData[] { });

                Verify<EventStoreContext>(c => c.SaveChangesAsync(), Times.Once());
            }

            [TestMethod, UnitTest]
            public async Task SaveAllAsync_Uses_AddRange_From_EventStoreContext_Events_With_Events_Without_Payload_When_Type_Is_Sensitive()
            {
                var events = Mock.Of<DbSet<Event>>();

                GetMock<EventStoreContext>()
                    .SetupGet(c => c.Events)
                    .Returns(events);

                var apiEventData = BuilderCollection
                    .Generate(() => new SensitiveApiEventData())
                    .ToList();

                await _sut.SaveAllAsync(apiEventData);

                Mock.Get(events).Verify(
                    e => e.AddRange(It.Is<IEnumerable<Event>>(collection =>
                        collection.All(item => apiEventData.Any(@event =>
                            item.Payload.IsNull() &&
                            item.AggregateID == @event.Id &&
                            item.TimeStamp == @event.TimeStamp &&
                            item.Type == @event.GetType().FullName &&
                            item.Version == @event.Version)))),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public async Task SaveAllAsync_Uses_AddRange_From_EventStoreContext_Events_With_Events_With_Payload_When_Type_Is_Regular()
            {
                var events = Mock.Of<DbSet<Event>>();

                GetMock<EventStoreContext>()
                    .SetupGet(c => c.Events)
                    .Returns(events);

                var apiEventData = BuilderCollection
                    .Generate(() => new RegularApiEventData(ObjectMother.Create<string>()))
                    .ToList();

                await _sut.SaveAllAsync(apiEventData);

                Mock.Get(events).Verify(
                    e => e.AddRange(It.Is<IEnumerable<Event>>(collection =>
                        collection.All(item => apiEventData.Any(@event =>
                            item.Payload.IsNotNull() &&
                            item.AggregateID == @event.Id &&
                            item.TimeStamp == @event.TimeStamp &&
                            item.Type == @event.GetType().FullName &&
                            item.Version == @event.Version)))),
                    Times.Once());
            }
        }
    }
}