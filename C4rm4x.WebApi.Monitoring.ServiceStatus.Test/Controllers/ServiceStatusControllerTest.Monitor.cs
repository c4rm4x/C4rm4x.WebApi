#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Monitoring.Core.Controllers;
using C4rm4x.WebApi.Monitoring.Core.Test.Controllers;
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
        public class ServiceStatusControllerMonitorTest :
            AutoMockFixture<ServiceStatusController>
        {
            private static List<IServiceStatusRetriever>
                _serviceStatusRetrievers;

            [TestInitialize]
            public override void Setup()
            {
                InitServiceStatusRetrievers();

                BuildContainer(new KeyValuePair<Type, Mock>(
                    typeof(IEnumerable<IServiceStatusRetriever>),
                    GetMockServiceStatusRetrieversEnumerable()));
            }

            [TestMethod, UnitTest]
            public void Monitor_Returns_BadRequest_Response_When_Request_Components_Contains_At_Least_One_Invalid_Component()
            {
                Assert.AreEqual(
                    HttpStatusCode.BadRequest,
                    Monitor(new MonitorRequestBuilder()
                        .WithComponents(new ComponentDtoBuilder()
                            .WithoutIdentifier()
                            .WithoutName()
                            .Build())
                        .Build()).Result.StatusCode);
            }    
            
            [TestMethod, UnitTest]
            public void Monitor_Uses_All_Instances_Of_IServiceStatusRetriever_When_Request_Components_Is_An_Empty_Collection()
            {
                var result = Monitor(new MonitorRequestBuilder()
                    .WithoutComponents()
                    .Build()).Result;

                foreach (var serviceStatusRetriever in _serviceStatusRetrievers)
                    Mock.Get(serviceStatusRetriever)
                        .Verify(s => s.MonitorAsync(), Times.Once());
            }

            [TestMethod, UnitTest]
            public void Monitor_Returns_Ok_When_Everything_Goes_Well_While_Processing_Request_Where_Components_Is_An_Empty_Collection()
            {
                Assert.AreEqual(
                    HttpStatusCode.OK, 
                    Monitor(new MonitorRequestBuilder()
                        .WithoutComponents()
                        .Build()).Result.StatusCode);
            }   
            
            [TestMethod, UnitTest]
            public void Monitor_Returns_MonitorResponse_Where_Each_Result_Component_Is_The_Component_Of_Each_IServiceStatusRetriever_When_Request_Components_Is_An_Empty_Collection()
            {
                var result = Monitor(new MonitorRequestBuilder()
                        .WithoutComponents()
                        .Build()).Result;
                var content = result.Content as ObjectContent<MonitorResponse<ComponentStatusDto>>;
                var response = content.Value as MonitorResponse<ComponentStatusDto>;

                Assert.IsNotNull(response);

                foreach (var r in response.Results)
                {
                    var component = r.Component;
                    Assert.IsNotNull(component);
                    Assert.IsTrue(_serviceStatusRetrievers.Any(s =>
                        s.ComponentName == component.Name &&
                        s.ComponentIdentifier == component.Identifier));
                }
            }

            [TestMethod, UnitTest]
            public void Monitor_Does_Not_Use_Any_Instances_Of_IServiceStatusRetriever_When_Request_Components_Is_Not_An_Empty_Collection_But_No_IServiceStatusRetriever_Is_Responsible_Of_Those()
            {
                var result = Monitor(new MonitorRequestBuilder()
                    .WithComponents(GetComponents().ToArray())
                    .Build()).Result;

                foreach (var serviceStatusRetriever in _serviceStatusRetrievers)
                    Mock.Get(serviceStatusRetriever)
                        .Verify(s => s.MonitorAsync(), Times.Never());
            }

            [TestMethod, UnitTest]
            public void Monitor_Uses_All_Instances_Of_IServicesStatusRetriever_That_Are_Responsible_For_All_Requested_Components()
            {
                var result = Monitor(new MonitorRequestBuilder()
                    .WithComponents(
                        _serviceStatusRetrievers.Select(s => 
                            new ComponentDto(s.ComponentIdentifier, s.ComponentName)).ToArray())
                    .Build()).Result;

                foreach (var serviceStatusRetriever in _serviceStatusRetrievers)
                    Mock.Get(serviceStatusRetriever)
                        .Verify(s => s.MonitorAsync(), Times.Once());
            }

            [TestMethod, UnitTest]
            public void Monitor_Returns_Ok_When_Everything_Goes_Well_While_Processing_Request_Where_Components_Is_Not_An_Empty_Collection()
            {
                Assert.AreEqual(
                    HttpStatusCode.OK,
                    Monitor(new MonitorRequestBuilder()
                        .WithComponents(GetComponents().ToArray())
                        .Build()).Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void Monitor_Returns_MonitorResponse_Where_Each_Result_Contains_An_Entry_For_Each_Requested_Component_When_Request_Components_Is_Not_An_Empty_Collection()
            {
                var components = GetComponents().ToArray();
                var result = Monitor(new MonitorRequestBuilder()
                        .WithComponents(components)
                        .Build()).Result;
                var content = result.Content as ObjectContent<MonitorResponse<ComponentStatusDto>>;
                var response = content.Value as MonitorResponse<ComponentStatusDto>;

                Assert.IsNotNull(response);

                foreach (var component in components)
                    Assert.IsTrue(response.Results.Any(c =>
                        c.Component.Identifier == component.Identifier &&
                        c.Component.Name == component.Name));
            }

            private static int GetRand(int max)
            {
                return new Random().Next(2, max);
            }

            private static void InitServiceStatusRetrievers()
            {
                _serviceStatusRetrievers = GetServiceStatusRetrievers().ToList();
            }

            private static IEnumerable<IServiceStatusRetriever> GetServiceStatusRetrievers()
            {
                var numberOfRetrievers = GetRand(10);

                for (var i = 0; i < numberOfRetrievers; i++)
                    yield return GetServiceStatusRetriever();
            }

            private static IServiceStatusRetriever GetServiceStatusRetriever()
            {
                var serviceStatusRetriever = Mock.Of<IServiceStatusRetriever>();

                Mock.Get(serviceStatusRetriever)
                    .SetupGet(s => s.ComponentIdentifier)
                    .Returns(ObjectMother.Create(20));

                Mock.Get(serviceStatusRetriever)
                    .SetupGet(s => s.ComponentName)
                    .Returns(ObjectMother.Create(50));

                Mock.Get(serviceStatusRetriever)
                    .Setup(s => s.MonitorAsync())
                    .Returns(Task.FromResult(ObjectMother.Create<bool>()));

                return serviceStatusRetriever;
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
                    .Returns(() => _serviceStatusRetrievers.GetEnumerator());

                return mockSequence;
            }

            private Task<HttpResponseMessage> Monitor(MonitorRequest request)
            {
                return _sut.Monitor(request)
                    .Result
                    .ExecuteAsync(It.IsAny<CancellationToken>());
            }
        }
    }
}
