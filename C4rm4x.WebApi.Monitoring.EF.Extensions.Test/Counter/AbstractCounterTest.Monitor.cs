#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Monitoring.EF.Test
{
    public partial class AbstractCounterTest
    {
        [TestClass]
        public class AbstractCounterMonitorTest : 
            AbstractCounterFixture
        {
            [TestMethod, IntegrationTest]
            public void Monitor_Returns_The_Total_Number_Of_Instances_Stored()
            {
                Assert.AreEqual(10, _sut.Monitor());
            }
        }
    }
}
