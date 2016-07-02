#region Using

using C4rm4x.WebApi.Monitoring.ServiceBus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceBus.Test
{
    public partial class AbstractCounterTest
    {
        #region Helper classes

        public class TestCounter :
            AbstractCounter
        {
            public TestCounter(
                ITopicDescriptionRetriever topicDescriptionRetriever) : 
                base("testCounter", "testCounter", "test", topicDescriptionRetriever)
            {
            }
        }

        #endregion

        [TestClass]
        public abstract class AbstractCounterFixture :
            BaseServiceBusFixture<TestCounter>
        {
            public AbstractCounterFixture() : 
                base("test")
            {
            }
        }
    }
}
