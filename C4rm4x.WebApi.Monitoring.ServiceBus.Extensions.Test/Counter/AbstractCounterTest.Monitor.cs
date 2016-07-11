#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceBus.Test
{
    public partial class AbstractCounterTest
    {
        [TestClass]
        public class AbstractCounterMonitorTest :
            AbstractCounterFixture
        {
            [TestMethod, IntegrationTest]
            public void Monitor_Returns_0_When_No_Messages_Are_Pending_To_Be_Processed_In_The_Topic()
            {
                Assert.AreEqual(0, _sut.Monitor());
            }

            [TestMethod, IntegrationTest]
            public void Monitor_Returns_The_Actual_Size_Of_The_Topic_In_Megabytes()
            {
                PushMessages(GetMessages());

                Assert.IsTrue(_sut.Monitor() > 0);
            }

            private static IEnumerable<TestMessage> GetMessages()
            {
                var numberOfMessages = GetRand(10);

                for (var i = 0; i < numberOfMessages; i++)
                    yield return new TestMessage(ObjectMother.Create<string>());
            }

            private static int GetRand(int max)
            {
                return new Random().Next(1, max);
            }
        }
    }
}
