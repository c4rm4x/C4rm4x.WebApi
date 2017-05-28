#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Monitoring.Redis.Test
{
    public partial class AbstractServiceStatusRetrieverTest
    {
        [TestClass]
        public class AbstractServiceStatusRetrieverMonitorAsyncTrueTest :
            AbstractServiceStatusRetrieverFixture
        {
            public AbstractServiceStatusRetrieverMonitorAsyncTrueTest()
                : base()
            {
            }

            [TestMethod, IntegrationTest]
            public async Task MonitorAsync_Returns_True_When_Redis_Is_Up_And_Running()
            {
                Assert.IsTrue(await _sut.MonitorAsync());
            }
        }

        [TestClass]
        public class AbstractServiceStatusRetrieverMonitorAsyncFalseTest :
            AbstractServiceStatusRetrieverFixture
        {
            public AbstractServiceStatusRetrieverMonitorAsyncFalseTest()
                : base(ObjectMother.Create(100)) // Any connection string
            {
            }

            [TestMethod, IntegrationTest]
            public async Task MonitorAsync_Returns_False_When_Redis_Is_Not_Up_And_Running()
            {
                Assert.IsFalse(await _sut.MonitorAsync());
            }
        }
    }
}
