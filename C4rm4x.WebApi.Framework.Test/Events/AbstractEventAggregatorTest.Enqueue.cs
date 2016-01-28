#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Events
{
    public partial class AbstractEventAggregatorTest
    {
        [TestClass]
        public class AbtractEventAggregatorEnqueueTest
        {
            [TestMethod, UnitTest]
            public void Enqueue_Adds_A_New_EventData_Into_The_Queue()
            {
                var sut = CreateSubjectUnderTest();

                sut.Enqueue(new TestEventData());

                Assert.IsTrue(sut.DataQueue.Any());
                Assert.AreEqual(1, sut.DataQueue.Count());
                Assert.IsInstanceOfType(sut.DataQueue.First(), typeof(TestEventData));
            }
        }
    }
}
