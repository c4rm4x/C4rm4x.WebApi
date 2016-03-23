#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Monitoring.Redis.Test
{
    public partial class AbstractServiceStatusRetrieverTest
    {
        [TestClass]
        public class AbstractServiceStatusRetrieverMonitorTrueTest :
            AbstractServiceStatusRetrieverFixture
        {
            public AbstractServiceStatusRetrieverMonitorTrueTest()
                : base()
            {
            }

            [TestMethod, IntegrationTest]
            public void Monitor_Returns_True_When_Redis_Is_Up_And_Running()
            {
                Assert.IsTrue(_sut.Monitor());
            }
        }

        [TestClass]
        public class AbstractServiceStatusRetrieverMonitorFalseTest :
            AbstractServiceStatusRetrieverFixture
        {
            public AbstractServiceStatusRetrieverMonitorFalseTest()
                : base(ObjectMother.Create(100)) // Any connection string
            {
            }

            [TestMethod, IntegrationTest]
            public void Monitor_Returns_False_When_Redis_Is_Not_Up_And_Running()
            {
                Assert.IsFalse(_sut.Monitor());
            }
        }
    }
}
