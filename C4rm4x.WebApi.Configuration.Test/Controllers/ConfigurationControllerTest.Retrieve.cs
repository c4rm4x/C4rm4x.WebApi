#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Configuration.Controllers;
using C4rm4x.WebApi.Configuration.Test.Controllers.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Configuration.Test.Controllers
{
    public partial class ConfigurationControllerTest
    {
        [TestClass]
        public class ConfigurationControllerRetrieveTest :
            AutoMockFixture<ConfigurationController>
        {
            [TestInitialize]
            public override void Setup()
            {
                base.Setup();

                SetupHttpContext();

                Returns<IAppConfigurationRepository, Task<AppConfiguration>>(
                    r => r.GetConfigurationAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Task.FromResult(new AppConfigurationBuilder().Build()));
            }

            [TestMethod, UnitTest]
            public void Retrieve_Returns_BadRequest_Response_When_Request_AppIdentifier_Is_Null()
            {
                Assert.AreEqual(
                    HttpStatusCode.BadRequest,
                    Retrieve(new GetConfigurationRequestBuilder().WithoutAppIdentifier().Build())
                        .Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void Retrieve_Returns_BadRequest_Response_When_Request_AppIdentifier_Is_Empty_String()
            {
                Assert.AreEqual(
                    HttpStatusCode.BadRequest,
                    Retrieve(new GetConfigurationRequestBuilder().WithAppIdentifier(string.Empty).Build())
                        .Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void Retrieve_Returns_BadRequest_Response_When_Request_Version_Is_Null()
            {
                Assert.AreEqual(
                    HttpStatusCode.BadRequest,
                    Retrieve(new GetConfigurationRequestBuilder().WithoutVersion().Build())
                        .Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void Retrieve_Returns_BadRequest_Response_When_Request_Version_Is_Empty_String()
            {
                Assert.AreEqual(
                    HttpStatusCode.BadRequest,
                    Retrieve(new GetConfigurationRequestBuilder().WithVersion(string.Empty).Build())
                        .Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void Retrieve_Uses_GetConfigurationAsync_From_IAppConfigurationRepository_To_Retrieve_The_Configuration()
            {
                var request = new GetConfigurationRequestBuilder().Build();
                var result = Retrieve(request);

                Verify<IAppConfigurationRepository>(
                    r => r.GetConfigurationAsync(request.AppIdentifier, request.Version),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void Retrieve_Returns_NotFound_Response_When_AppConfiguration_Cannot_Be_Found()
            {
                Returns<IAppConfigurationRepository, Task<AppConfiguration>>(
                    r => r.GetConfigurationAsync(It.IsAny<string>(), It.IsAny<string>()),
                    Task.FromResult(null as AppConfiguration));

                Assert.AreEqual(
                    HttpStatusCode.NotFound,
                    Retrieve(new GetConfigurationRequestBuilder().Build()).Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void Retrieve_Returns_Ok_Response_When_AppConfiguration_For_Given_Request_Exists()
            {
                Assert.AreEqual(
                    HttpStatusCode.OK,
                    Retrieve(new GetConfigurationRequestBuilder().Build())
                        .Result.StatusCode);
            }

            private Task<HttpResponseMessage> Retrieve(GetConfigurationRequest request)
            {
                return _sut.Retrieve(request)
                    .Result
                    .ExecuteAsync(It.IsAny<CancellationToken>());
            }

            private void SetupHttpContext()
            {
                _sut.Request = new HttpRequestMessage();
            }
        }
    }
}
