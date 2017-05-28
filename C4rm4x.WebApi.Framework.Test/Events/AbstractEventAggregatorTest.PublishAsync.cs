#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Events
{
    public partial class AbstractEventAggregatorTest
    {
        [TestClass]
        public class AbstractEventAggregatorPublishAsyncTest
        {
            [TestMethod, UnitTest]
            public async Task PublishAsync_Does_Not_Enqueue_A_New_EventData_Into_The_Queue()
            {
                var sut = CreateSubjectUnderTest();

                await sut.PublishAsync(new TestEventData());

                Assert.IsFalse(sut.DataQueue.Any());
            }

            [TestMethod, UnitTest]
            public async Task PublishAsync_Invokes_OnEventHandler_Of_Every_EventHandler_For_Such_Event_Type()
            {
                var eventData = new TestEventData();
                var handlers = GetHandlers<TestEventData>(GetRand(5))
                    .ToArray(); // Get the final list

                await CreateSubjectUnderTest(handlers)
                    .PublishAsync(eventData);

                foreach (var handler in handlers)
                    Mock.Get(handler)
                        .Verify(h => h.OnEventHandlerAsync(eventData), Times.Once);
            }
        }
    }
}
