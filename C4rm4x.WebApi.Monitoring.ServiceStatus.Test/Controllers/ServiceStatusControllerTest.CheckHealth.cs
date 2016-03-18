#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Test.Controllers
{
    public partial class ServiceStatusControllerTest
    {
        [TestClass]
        public class ServiceStatusControllerCheckHealthTest :
            AutoMockFixture<ServiceStatusController>
        {            
            [TestInitialize]
            public override void Setup()
            {
                BuildContainer(new KeyValuePair<Type, Mock>(
                    typeof(IEnumerable<IServiceStatusRetriever>),
                    GetMockServiceStatusRetrieversEnumerable()));
            }

            [TestMethod, UnitTest]
            public void CheckHealth_Returns_BadRequest_Response_When_Request_Components_Contains_At_Least_One_Invalid_Component()
            {
                Assert.AreEqual(
                    HttpStatusCode.BadRequest,
                    CheckHealth(new CheckHealthRequestBuilder()
                        .WithComponents(new ComponentDtoBuilder()
                            .WithoutIdentifier()
                            .WithoutName()
                            .Build())
                        .Build()).Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void CheckHealth_Sets_All_Instances_Of_IServiceStatusRetrievers_To_OverallServiceStatusHandler_When_Request_Components_Is_An_Empty_Collection()
            {
                var overralServiceStatusRequestHandler = GetServiceStatusRequestHandler();

                _sut.SetOverallServiceStatusHandlerFactory(
                    () => overralServiceStatusRequestHandler);

                CheckHealth(new CheckHealthRequestBuilder()
                    .WithoutComponents()
                    .Build());

                Mock.Get(overralServiceStatusRequestHandler)
                    .Verify(h => h.SetServiceStatusRetrievers(
                        It.IsAny<IEnumerable<IServiceStatusRetriever>>()), 
                        Times.Once());
            }

            [TestMethod, UnitTest]
            public void CheckHealth_Invokes_Handle_From_OverallServiceStatusHandler_When_Request_Components_Is_An_Empty_Collection()
            {
                var request = new CheckHealthRequestBuilder()
                    .WithoutComponents()
                    .Build();

                var overralServiceStatusRequestHandler = GetServiceStatusRequestHandler();

                _sut.SetOverallServiceStatusHandlerFactory(
                    () => overralServiceStatusRequestHandler);

                CheckHealth(request);

                Mock.Get(overralServiceStatusRequestHandler)
                    .Verify(h => h.Handle(request), Times.Once());
            }

            [TestMethod, UnitTest]
            public void CheckHealth_Returns_The_Result_Created_By_OverallServiceStatusHandler_When_Request_Components_Is_An_Empty_Collection()
            {
                var response = new CheckHealthResponseBuilder()
                    .Build();

                var overralServiceStatusRequestHandler = GetServiceStatusRequestHandler(response);

                _sut.SetOverallServiceStatusHandlerFactory(
                    () => overralServiceStatusRequestHandler);

                var result = CheckHealth(new CheckHealthRequestBuilder()
                    .WithoutComponents()
                    .Build()).Result;
                var content = result.Content as ObjectContent<CheckHealthResponse>;

                Assert.AreEqual(response, content.Value as CheckHealthResponse);
            }

            [TestMethod, UnitTest]
            public void CheckHealth_Sets_All_Instances_Of_IServiceStatusRetrievers_To_ByComponentServiceStatusHandler_When_Request_Components_Is_Not_An_Empty_Collection()
            {
                var byComponentServiceStatusRequestHandler = GetServiceStatusRequestHandler(); ;

                _sut.SetByComponentServiceStatusHandlerFactory(
                    () => byComponentServiceStatusRequestHandler);

                CheckHealth(new CheckHealthRequestBuilder()
                    .WithComponents(GetComponents().ToArray())
                    .Build());

                Mock.Get(byComponentServiceStatusRequestHandler)
                    .Verify(h => h.SetServiceStatusRetrievers(
                        It.IsAny<IEnumerable<IServiceStatusRetriever>>()), 
                        Times.Once());
            }

            [TestMethod, UnitTest]
            public void CheckHealth_Invokes_Handle_From_ByComponentServiceStatusHandler_When_Request_Components_Is_Not_An_Empty_Collection()
            {
                var request = new CheckHealthRequestBuilder()
                    .WithComponents(GetComponents().ToArray())
                    .Build();

                var byComponentServiceStatusRequestHandler = GetServiceStatusRequestHandler();

                _sut.SetByComponentServiceStatusHandlerFactory(
                    () => byComponentServiceStatusRequestHandler);

                CheckHealth(request);

                Mock.Get(byComponentServiceStatusRequestHandler)
                    .Verify(h => h.Handle(request), Times.Once());
            }

            [TestMethod, UnitTest]
            public void CheckHealth_Returns_The_Result_Created_By_ByComponentServiceStatusHandler_When_Request_Components_Is_Not_An_Empty_Collection()
            {
                var response = new CheckHealthResponseBuilder()
                    .Build();

                var byComponentServiceStatusRequestHandler = GetServiceStatusRequestHandler(response);

                _sut.SetByComponentServiceStatusHandlerFactory(
                    () => byComponentServiceStatusRequestHandler);

                var result = CheckHealth(new CheckHealthRequestBuilder()
                    .WithComponents(GetComponents().ToArray())
                    .Build()).Result;
                var content = result.Content as ObjectContent<CheckHealthResponse>;

                Assert.AreEqual(response, content.Value as CheckHealthResponse);
            }

            private static int GetRand(int max)
            {
                return new Random().Next(1, max);
            }

            private static IEnumerable<IServiceStatusRetriever> GetServiceStatusRetrievers()
            {
                var numberOfRetrievers = GetRand(10);

                for (var i = 0; i < numberOfRetrievers; i++)
                    yield return Mock.Of<IServiceStatusRetriever>();
            }

            private static IEnumerable<ComponentDto> GetComponents()
            {
                var numberOfRetrievers = GetRand(10);

                for (var i = 0; i < numberOfRetrievers; i++)
                    yield return new ComponentDtoBuilder().Build();
            }

            private static Mock GetMockServiceStatusRetrieversEnumerable()
            {
                var mockSequence = new Mock<IEnumerable<IServiceStatusRetriever>>();

                mockSequence
                    .Setup(m => m.GetEnumerator())
                    .Returns(GetServiceStatusRetrievers().GetEnumerator());

                return mockSequence;
            }

            private static IServiceStatusRequestHandler GetServiceStatusRequestHandler(
                CheckHealthResponse response = null)
            {
                var serviceStatusRequestHandler = Mock.Of<IServiceStatusRequestHandler>();

                Mock.Get(serviceStatusRequestHandler)
                    .Setup(s => s.Handle(It.IsAny<CheckHealthRequest>()))
                    .Returns(new OkResult<CheckHealthResponse>(
                        response ?? new CheckHealthResponseBuilder().Build()));

                return serviceStatusRequestHandler;
            }

            private Task<HttpResponseMessage> CheckHealth(CheckHealthRequest request)
            {
                return _sut.CheckHealth(request)
                    .ExecuteAsync(It.IsAny<CancellationToken>());
            }
        }
    }
}
