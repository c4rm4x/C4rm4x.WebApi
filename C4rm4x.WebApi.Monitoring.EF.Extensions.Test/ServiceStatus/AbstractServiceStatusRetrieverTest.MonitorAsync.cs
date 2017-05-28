#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Monitoring.EF.Test
{
    public partial class AbstractServiceStatusRetrieverTest
    {
        [TestClass]
        public class AbstractServiceStatusRetrieverMonitorAsyncTest :
            AbstractServiceStatusRetrieverFixture
        {
            [TestMethod, IntegrationTest]
            public async Task MonitorAsync_Returns_True_When_A_Connection_With_Database_Can_Be_Established()
            {
                Assert.IsTrue(await _sut.MonitorAsync());
            }
        }
    }
}
