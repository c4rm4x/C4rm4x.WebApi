#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceBus.Test
{
    public partial class AbstractServiceStatusRetrieverTest
    {
        [TestClass]
        public class AbstractServiceStatusRetrieverMonitorTest :
            AbstractServiceStatusRetrieverFixture
        {
            [TestMethod, IntegrationTest]
            public void Monitor_Returns_True_When_TopicDescription_Can_Be_Retrieved()
            {
                Assert.IsTrue(_sut.Monitor());
            }
        }
    }
}
