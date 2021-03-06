﻿#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Monitoring.AzureQueue.Test
{
    public partial class AbstractCounterTest
    {
        [TestClass]
        public class AbstractCounterMonitorAsyncTest :
            AbstractCounterFixture
        {
            [TestMethod, IntegrationTest]
            public async Task MonitorAsync_Returns_0_When_No_Messages_Are_Pending_To_Be_Processed_In_The_Queue()
            {
                Assert.AreEqual(0, await _sut.MonitorAsync());
            }

            [TestMethod, IntegrationTest]
            public async Task MonitorAsync_Returns_The_Actual_Number_Of_Messages_In_The_Queue()
            {
                PushMessages(GetMessages());

                Assert.IsTrue(await _sut.MonitorAsync() > 0);
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
