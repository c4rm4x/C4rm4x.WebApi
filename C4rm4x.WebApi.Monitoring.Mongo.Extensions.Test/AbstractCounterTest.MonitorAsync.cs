#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Monitoring.Mongo.Test
{
    public partial class AbstractCounterTest
    {
        [TestClass]
        public class AbstractCounterMonitorAsyncTest :
            AbstractCounterFixture
        {
            [TestMethod, IntegrationTest]
            public async Task MonitorAsync_Returns_The_Total_Number_Of_Instances_Stored()
            {
                Assert.AreEqual(10, await _sut.MonitorAsync());
            }
        }
    }
}
