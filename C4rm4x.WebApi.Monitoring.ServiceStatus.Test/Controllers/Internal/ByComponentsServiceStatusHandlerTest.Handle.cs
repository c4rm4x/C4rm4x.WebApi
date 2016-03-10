#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Internal;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Test.Controllers.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Test.Controllers.Internal
{
    public partial class ByComponentsServiceStatusHandlerTest
    {
        [TestClass]
        public class ByComponentsServiceStatusHandlerHandleTest :
            AbstractServiceStatusHandlerTest
        {
            public object ComponenHealthStatus { get; private set; }

            [TestMethod, UnitTest]
            public void Handle_Returns_Ok_Response_When_Everything_Goes_Ok()
            {
                Assert.AreEqual(
                    HttpStatusCode.OK,
                    Handle().Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void Handle_Returns_An_Instance_Of_CheckHealthResponse_With_The_Collection_Of_All_ComponentStatuses_As_ComponentHealthStatus_Unknown_When_No_Instance_Of_IServiceStatusRetriever_Is_Responsible_For_Those()
            {
                var components = GetComponents().ToArray();
                var request = new CheckHealthRequestBuilder()
                        .WithComponents(components)
                        .Build();

                var content = Handle(request: request).Result.Content;
                Assert.IsInstanceOfType(
                    content,
                    typeof(ObjectContent<CheckHealthResponse>));

                var value = (content as ObjectContent<CheckHealthResponse>).Value;
                Assert.IsInstanceOfType(
                    value,
                    typeof(CheckHealthResponse));

                var response = value as CheckHealthResponse;                
                Assert.AreEqual(components.Count(), response.ComponentStatuses.Count());

                foreach (var component in components)
                {
                    var componentStatus = response
                        .ComponentStatuses
                        .FirstOrDefault(c => c.Component.Identifier == component.Identifier);

                    Assert.IsNotNull(componentStatus);
                    Assert.AreSame(component, componentStatus.Component);
                    Assert.AreEqual(ComponentHealthStatus.Unknown, componentStatus.HealthStatus);
                }
            }

            [TestMethod, UnitTest]
            public void Handle_Uses_No_Instance_Of_IServiceStatusRetriever_When_None_Is_Responsible_For_Any_Component_Requested()
            {
                var retrievers = GetRetrievers().ToArray();

                Handle(request: new CheckHealthRequestBuilder()
                    .WithComponents(GetComponents().ToArray())
                    .Build(),
                    retrievers: retrievers);

                foreach (var retriever in retrievers)
                    Mock.Get(retriever)
                        .Verify(r => r.IsComponentWorking(), Times.Never());
            }

            [TestMethod, UnitTest]
            public void Handle_Uses_The_Instances_Of_IServiceStatusRetrieve_That_Are_Responsible_For_All_Requested_Components()
            {                
                var components = GetComponents().ToArray();
                var retrievers = GetRetrievers(components).ToArray();

                Handle(request: new CheckHealthRequestBuilder()
                    .WithComponents(components)
                    .Build(),
                    retrievers: retrievers);

                foreach (var retriever in retrievers)
                    Mock.Get(retriever)
                        .Verify(r => r.IsComponentWorking(), Times.Once());
            }            

            protected override object GetServiceStatusRequestHandler()
            {
                return ByComponentsServiceStatusHandler.GetInstance();
            }

            private static IEnumerable<ComponentDto> GetComponents()
            {
                var numberOfComponents = GetRand(10);

                for (var i = 0; i < numberOfComponents; i++)
                    yield return GetComponent();
            }

            private static ComponentDto GetComponent(object identifier = null)
            {
                return new ComponentDtoBuilder()
                        .WithIdentifier(identifier ?? ObjectMother.Create(10))
                        .Build();
            }

            private static IEnumerable<IServiceStatusRetriever> GetRetrievers(
                params ComponentDto[] components)
            {
                foreach (var component in components)
                    yield return GetServiceStatusRetriever(
                        component.Identifier.ToString(), component.Name);
            }
        }
    }
}
