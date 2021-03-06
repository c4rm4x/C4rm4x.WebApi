﻿#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache.Test
{
    public partial class AutoInvalidateOutputCacheAttributeTest
    {
        [TestClass]
        public class AutoInvalidateOutputCacheAttributeOnActionExecutedAsyncTest
        {
            #region Helper classes

            private class TestController : ApiController
            {
                public IHttpActionResult Get()
                {
                    throw new NotImplementedException();
                }

                [HttpGet]
                public IHttpActionResult NotGet()
                {
                    throw new NotImplementedException();
                }
            }

            #endregion

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Does_Not_Check_Cache_When_Response_Is_Was_Not_Successfully_Processed()
            {
                var cache = GetCache();
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(GetHttpActionExecutedContext(cache, response), It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.ExistsAsync(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Does_Not_Check_Cache_When_Response_Is_Was_Not_Successfully_Processed_But_Request_Is_An_Http_Get()
            {
                var cache = GetCache();

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(GetHttpActionExecutedContext(HttpMethod.Get, cache), It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.ExistsAsync(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Does_Not_Check_Cache_When_Response_Is_Was_Not_Successfully_Processed_But_Request_Is_An_Http_Head()
            {
                var cache = GetCache();

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(GetHttpActionExecutedContext(HttpMethod.Head, cache), It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.ExistsAsync(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Does_Not_Check_Cache_When_Response_Is_Was_Not_Successfully_Processed_But_Request_Is_An_Http_Options()
            {
                var cache = GetCache();

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(GetHttpActionExecutedContext(HttpMethod.Options, cache), It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.ExistsAsync(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Does_Not_Check_Cache_When_Response_Is_Was_Not_Successfully_Processed_But_Request_Is_An_Http_Trace()
            {
                var cache = GetCache();

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(GetHttpActionExecutedContext(HttpMethod.Trace, cache), It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.ExistsAsync(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Checks_Whether_Or_Not_The_Key_For_The_Given_Action_Name_Is_In_The_Cache_When_Response_Was_Successfully_Processed_And_Request_Is_An_Http_Post_For_Each_Get_Method()
            {
                var cache = GetCache();

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(GetHttpActionExecutedContext(HttpMethod.Post, cache), It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.ExistsAsync(It.IsAny<string>()), Times.Exactly(2));
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Checks_Whether_Or_Not_The_Key_For_The_Given_Action_Name_Is_In_The_Cache_When_Response_Was_Successfully_Processed_And_Request_Is_An_Http_Put_For_Each_Get_Method()
            {
                var cache = GetCache();

               await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(GetHttpActionExecutedContext(HttpMethod.Put, cache), It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.ExistsAsync(It.IsAny<string>()), Times.Exactly(2));
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Checks_Whether_Or_Not_The_Key_For_The_Given_Action_Name_Is_In_The_Cache_When_Response_Was_Successfully_Processed_And_Request_Is_An_Http_Delete_For_Each_Get_Method()
            {
                var cache = GetCache();

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(GetHttpActionExecutedContext(HttpMethod.Delete, cache), It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.ExistsAsync(It.IsAny<string>()), Times.Exactly(2));
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Does_Not_Try_To_Remove_The_Entries_From_The_Cache_When_The_Keys_Are_Not_Stored()
            {
                var cache = GetCache();

                Mock.Get(cache)
                    .Setup(c => c.ExistsAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(false));

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(GetHttpActionExecutedContext(cache), It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.RemoveAsync(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Tries_To_Remove_The_Entries_From_The_Cache_When_The_Keys_Are_Stored()
            {
                var cache = GetCache();

                Mock.Get(cache)
                    .Setup(c => c.ExistsAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(GetHttpActionExecutedContext(cache), It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.RemoveAsync(It.IsAny<string>()), Times.Exactly(2));
            }

            private static AutoInvalidateOutputCacheAttribute CreateSubjectUnderTest()
            {
                return new AutoInvalidateOutputCacheAttribute();
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
                return GetHttpActionExecutedContext(HttpMethod.Post, cache, response);
            }

            private static HttpActionExecutedContext GetHttpActionExecutedContext(
                HttpMethod method,
                ICache cache = null,
                HttpResponseMessage response = null)
            {
                var actionExecutedContext = new HttpActionExecutedContext(
                    GetHttpActionContext(cache, method), null);

                actionExecutedContext.Response =
                    actionExecutedContext.ActionContext.Response =
                    response ?? GetHttpResponseMessage();

                return actionExecutedContext;
            }

            private static HttpActionContext GetHttpActionContext(
                ICache cache,
                HttpMethod method)
            {
                return new HttpActionContext(
                    GetHttpControllerContext(cache ?? GetCache(), method),
                    GetHttpActionDescriptor());
            }

            private static HttpControllerContext GetHttpControllerContext(
                ICache cache,
                HttpMethod method)
            {
                var config = GetHttpConfiguration(() => cache);
                var request = GetHttpRequestMessage(method, config);
                var controllerContext = new HttpControllerContext(
                    config,
                    GetHttpRouteData(config, request),
                    request);

                controllerContext.ControllerDescriptor =
                    GetHttpControllerDescriptor(config);

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
                HttpMethod method,
                HttpConfiguration config)
            {
                var request = new HttpRequestMessage(
                    method, "http://localhost/api/test");

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

            private static HttpActionDescriptor GetHttpActionDescriptor()
            {
                return Mock.Of<HttpActionDescriptor>();
            }
        }
    }
}
