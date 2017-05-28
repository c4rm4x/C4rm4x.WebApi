#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceBus.Test
{
    public partial class AbstractServiceStatusRetrieverTest
    {
        [TestClass]
        public class AbstractServiceStatusRetrieverMonitorAsyncTest :
            AbstractServiceStatusRetrieverFixture
        {
            [TestMethod, IntegrationTest]
            public async Task MonitorAsync_Returns_True_When_TopicDescription_Can_Be_Retrieved()
            {
                Assert.IsTrue(await _sut.MonitorAsync());
            }
        }
    }
}
