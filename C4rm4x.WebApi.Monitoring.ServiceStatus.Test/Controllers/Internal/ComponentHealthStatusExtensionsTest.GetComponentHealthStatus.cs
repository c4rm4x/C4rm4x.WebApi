#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Test.Controllers
{
    public partial class ComponentHealthStatusExtensionsTest
    {
        [TestClass]
        public class ComponentHealthStatusExtensionsGetComponentHealthStatusTest
        {
            [TestMethod, UnitTest]
            public void GetComponentHealthStatus_Returns_ComponentHealthStatus_Working_When_IsComponentWorking_Is_True()
            {
                Assert.AreEqual(
                    ComponentHealthStatus.Working,
                    GetServiceStatusRetriever(true)
                        .GetComponentHealthStatus());
            }

            [TestMethod, UnitTest]
            public void GetComponentHealthStatus_Returns_ComponentHealthStatus_Unresponsive_When_IsComponentWorking_Is_False()
            {
                Assert.AreEqual(
                    ComponentHealthStatus.Unresponsive,
                    GetServiceStatusRetriever(false)
                        .GetComponentHealthStatus());
            }

            private IServiceStatusRetriever GetServiceStatusRetriever(bool isComponentWorking)
            {
                var retriever = Mock.Of<IServiceStatusRetriever>();

                Mock.Get(retriever)
                    .Setup(r => r.IsComponentWorking())
                    .Returns(isComponentWorking);

                return retriever;
            }
        }
    }
}
