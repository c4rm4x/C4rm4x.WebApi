#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Test
{
    public partial class AbstractServiceStatusRetrieverTest
    {
        [TestClass]
        public class AbstractServiceStatusRetrieverMonitorTest
        {
            #region Helper classes

            class TestServiceStatusRetriever : AbstractServiceStatusRetriever
            {
                public bool IsWorking { get; private set; }

                public TestServiceStatusRetriever(bool isWorking = true)
                    : base("ComponentIdentifier", "ComponentName")
                {
                    IsWorking = isWorking;
                }

                protected override async Task CheckComponentResponsivenessAsync()
                {
                    await Task.Run(() =>
                    {
                        if (!IsWorking)
                            throw new Exception();
                    });
                }
            }

            #endregion

            [TestMethod, UnitTest]
            public void Monitor_Returns_True_When_No_Exception_Is_Thrown()
            {
                Assert.IsTrue(
                    CreateSubjectUnderTest(true)
                        .MonitorAsync().Result);
            }

            [TestMethod, UnitTest]
            public void Monitor_Returns_False_When_Any_Exception_Is_Thrown()
            {
                Assert.IsFalse(
                    CreateSubjectUnderTest(false)
                        .MonitorAsync().Result);
            }

            private IServiceStatusRetriever CreateSubjectUnderTest(bool isComponentWorking)
            {
                return new TestServiceStatusRetriever(isComponentWorking);
            }
        }
    }
}
