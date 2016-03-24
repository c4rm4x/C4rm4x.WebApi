#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Monitoring.EF.Test
{
    public partial class AbstractServiceStatusRetrieverTest
    {
        [TestClass]
        public class AbstractServiceStatusRetrieverMonitorTest :
            AbstractServiceStatusRetrieverFixture
        {
            [TestMethod, UnitTest]
            public void Monitor_Returns_True_When_A_Connection_With_Database_Can_Be_Established()
            {
                Assert.IsTrue(_sut.Monitor());
            }
        }
    }
}
