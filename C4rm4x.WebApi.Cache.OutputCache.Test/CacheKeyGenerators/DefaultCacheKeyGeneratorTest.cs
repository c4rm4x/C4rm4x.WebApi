#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache.Test
{
    public partial class DefaultCacheKeyGeneratorTest
    {
        [TestClass]
        public class DefaultCacheKeyGeneratorGenerateTest :
            AutoMockFixture<DefaultCacheKeyGenerator>
        {
            #region Helper classes

            private class TestController : ApiController
            { }

            #endregion

            [TestMethod, UnitTest]
            public void Generate_Returns_Key_As_Combintation_Of_Controller_Classname_And_ActionName()
            {
                var ActionName = ObjectMother.Create<string>();

                Assert.AreEqual(
                    "{0}-{1}".AsFormat(typeof(TestController).FullName, ActionName),
                    _sut.Generate(GetHttpActionContext(ActionName)));
            }

            private HttpActionContext GetHttpActionContext(
                string actionName)
            {
                return new HttpActionContext(
                    GetHttpControllerContext(),
                    GetHttpActionDescriptor(actionName));
            }

            private static HttpControllerContext GetHttpControllerContext()
            {
                var config = GetHttpConfiguration();
                var request = GetHttpRequestMessage(config);
                var controllerContext = new HttpControllerContext(
                    config,
                    GetHttpRouteData(config, request),
                    request);

                controllerContext.ControllerDescriptor =
                    GetHttpControllerDescriptor(config);

                return controllerContext;
            }

            private static HttpConfiguration GetHttpConfiguration()
            {
                return new HttpConfiguration();
            }

            private static HttpRouteData GetHttpRouteData(
                HttpConfiguration config,
                HttpRequestMessage request)
            {
                var route = config.Routes.MapHttpRoute(
                    "default", "api/{controller}/{id}");

                return new HttpRouteData(
                    route,
                    new HttpRouteValueDictionary { { "controller", "test" } });
            }

            private static HttpRequestMessage GetHttpRequestMessage(
                HttpConfiguration config)
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Get, "http://localhost/api/test");

                request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

                return request;
            }

            private static HttpControllerDescriptor GetHttpControllerDescriptor(
                HttpConfiguration config)
            {
                return new HttpControllerDescriptor(
                    config,
                    "test",
                    typeof(TestController));
            }

            private static HttpActionDescriptor GetHttpActionDescriptor(
                string actionName)
            {
                var actionDescriptor = Mock.Of<HttpActionDescriptor>();

                Mock.Get(actionDescriptor)
                    .SetupGet(a => a.ActionName)
                    .Returns(actionName);

                return actionDescriptor;
            }
        }
    }
}
