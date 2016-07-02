#region Using

using C4rm4x.WebApi.Monitoring.ServiceBus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceBus.Test
{
    public partial class AbstractServiceStatusRetrieverTest
    {
        #region Helper classes

        public class TestServiceStatusRetriever :
            AbstractServiceStatusRetriever
        {
            public TestServiceStatusRetriever(
                ITopicDescriptionRetriever topicDescriptionRetriever)
                : base("testComponent", "testComponent", "test", topicDescriptionRetriever)
            {

            }
        }

        #endregion

        [TestClass]
        public abstract class AbstractServiceStatusRetrieverFixture :
            BaseServiceBusFixture<TestServiceStatusRetriever>
        {
            public AbstractServiceStatusRetrieverFixture() : 
                base("test")
            {
            }
        }
    }
}
