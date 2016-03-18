#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Net;
using System.Net.Http;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Test.Controllers
{
    public partial class OverallServiceStatusHandlerTest
    {
        [TestClass]
        public class OverallServiceStatusHandlerHandleTest :
            AbstractServiceStatusHandlerTest
        {
            [TestMethod, UnitTest]
            public void Handle_Returns_Ok_Response_When_Everything_Goes_Ok()
            {
                Assert.AreEqual(
                    HttpStatusCode.OK,
                    Handle().Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void Handle_Returns_An_Instance_Of_CheckHealthResponse_With_The_Collection_Of_All_ComponentStatuses_Returned_By_All_Instance_Of_IServiceStatusRetriever()
            {
                var retrievers = GetRetrievers().ToArray();

                var content = Handle(retrievers: retrievers).Result.Content;
                Assert.IsInstanceOfType(
                    content,
                    typeof(ObjectContent<CheckHealthResponse>));

                var value = (content as ObjectContent<CheckHealthResponse>).Value;
                Assert.IsInstanceOfType(
                    value,
                    typeof(CheckHealthResponse));

                var response = value as CheckHealthResponse;
                Assert.AreEqual(retrievers.Count(), response.ComponentStatuses.Count());
            }


            [TestMethod, UnitTest]
            public void Handle_Uses_All_Instance_Of_IServiceStatusRetriever()
            {
                var retrievers = GetRetrievers().ToArray();

                Handle(retrievers: retrievers); 

                foreach (var retriever in retrievers)
                    Mock.Get(retriever)
                        .Verify(r => r.IsComponentWorking(), Times.Once());
            }

            protected override object GetServiceStatusRequestHandler()
            {
                return OverallServiceStatusHandler.GetInstance();
            }
        }
    }
}
