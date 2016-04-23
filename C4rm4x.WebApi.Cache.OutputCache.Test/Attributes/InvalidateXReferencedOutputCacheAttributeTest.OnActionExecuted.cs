#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache.Test
{
    public partial class InvalidateXReferencedOutputCacheAttributeTest
    {
        [TestClass]
        public class InvalidateXReferencedOutputCacheAttributeOnActionExecutedTest
        {
            private const string ActionName = "actionName";

            #region Helper classes

            private class TestController : ApiController
            { }

            #endregion

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Check_Cache_When_Response_Is_Was_Not_Successfully_Processed()
            {
                var cache = GetCache();
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(cache, response));

                Mock.Get(cache)
                    .Verify(c => c.Exists(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Checks_Whether_Or_Not_The_Key_For_The_Given_ControllerType_And_Action_Name_Is_In_The_Cache_When_Response_Was_Successfully_Processed()
            {
                var cache = GetCache();
                var response = GetHttpResponseMessage();

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(cache, response));

                Mock.Get(cache)
                    .Verify(c => c.Exists(GetKey(typeof(TestController), ActionName)), Times.Once());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Try_To_Remove_The_Entry_From_The_Cache_When_The_Key_Is_Not_Stored()
            {
                var cache = GetCache();

                Mock.Get(cache)
                    .Setup(c => c.Exists(It.IsAny<string>()))
                    .Returns(false);

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(cache));

                Mock.Get(cache)
                    .Verify(c => c.Remove(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Tries_To_Remove_The_Entry_From_The_Cache_When_The_Key_Is_Stored()
            {
                var cache = GetCache();

                Mock.Get(cache)
                    .Setup(c => c.Exists(It.IsAny<string>()))
                    .Returns(true);

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(cache));

                Mock.Get(cache)
                    .Verify(c => c.Remove(GetKey(typeof(TestController), ActionName)), Times.Once());
            }

            private static InvalidateXReferencedOutputCacheAttribute CreateSubjectUnderTest(
                Type controllerType = null,
                string actionName = ActionName)
            {
                return new InvalidateXReferencedOutputCacheAttribute(
                    controllerType ?? typeof(TestController),
                    actionName);
            }

            protected static ICache GetCache()
            {
                return Mock.Of<ICache>();
            }

            private static HttpResponseMessage GetHttpResponseMessage(
                byte[] content = null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);

                if (!content.IsNullOrEmpty())
                    response.Content = new ByteArrayContent(content);

                return response;
            }

            private static HttpActionExecutedContext GetHttpActionExecutedContext(
                ICache cache = null,
                HttpResponseMessage response = null)
            {
                var actionExecutedContext = new HttpActionExecutedContext(
                    GetHttpActionContext(cache), null);

                actionExecutedContext.Response =
                    actionExecutedContext.ActionContext.Response =
                    response ?? GetHttpResponseMessage();

                return actionExecutedContext;
            }

            private static HttpActionContext GetHttpActionContext(
                ICache cache)
            {
                return new HttpActionContext(
                    GetHttpControllerContext(cache ?? GetCache()),
                    Mock.Of<HttpActionDescriptor>());
            }

            private static HttpControllerContext GetHttpControllerContext(
                ICache cache)
            {
                var config = GetHttpConfiguration(() => cache);
                var request = GetHttpRequestMessage(config);
                var controllerContext = new HttpControllerContext(
                    config,
                    GetHttpRouteData(config, request),
                    request);

                return controllerContext;
            }

            private static HttpConfiguration GetHttpConfiguration(
                Func<ICache> cacheRetriever)
            {
                var config = new HttpConfiguration();

                config.Properties.GetOrAdd(typeof(ICache), obj => cacheRetriever);

                return config;
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

            private static string GetKey(
                Type controllerType,
                string actionName)
            {
                return "{0}-{1}".AsFormat(controllerType.FullName, actionName);
            }
        }
    }
}
