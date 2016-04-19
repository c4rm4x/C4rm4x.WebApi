#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class ClientOnlyOutputCacheAttributeTest
    {
        [TestClass]
        public class ClientOnlyOutputCacheAttributeOnActionExecutedTest
        {
            private const int DefaultClientTimeSpan = 60;

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Set_CacheControl_Header_When_ActionExecutedContext_Response_Is_Not_Successfull()
            {
                var actionExecutedContext = GetHttpActionExecutedContext(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError));

                CreateSubjectUnderTest()
                    .OnActionExecuted(actionExecutedContext);

                Assert.IsNull(actionExecutedContext.Response.Headers.CacheControl);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Set_CacheControl_Header_When_HttpMethod_Is_Delete()
            {
                var actionExecutedContext = GetHttpActionExecutedContext(
                    HttpMethod.Delete);

                CreateSubjectUnderTest()
                    .OnActionExecuted(actionExecutedContext);

                Assert.IsNull(actionExecutedContext.Response.Headers.CacheControl);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Set_CacheControl_Header_When_HttpMethod_Is_Head()
            {
                var actionExecutedContext = GetHttpActionExecutedContext(
                    HttpMethod.Head);

                CreateSubjectUnderTest()
                    .OnActionExecuted(actionExecutedContext);

                Assert.IsNull(actionExecutedContext.Response.Headers.CacheControl);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Set_CacheControl_Header_When_HttpMethod_Is_Options()
            {
                var actionExecutedContext = GetHttpActionExecutedContext(
                    HttpMethod.Options);

                CreateSubjectUnderTest()
                    .OnActionExecuted(actionExecutedContext);

                Assert.IsNull(actionExecutedContext.Response.Headers.CacheControl);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Set_CacheControl_Header_When_HttpMethod_Is_Post()
            {
                var actionExecutedContext = GetHttpActionExecutedContext(
                    HttpMethod.Post);

                CreateSubjectUnderTest()
                    .OnActionExecuted(actionExecutedContext);

                Assert.IsNull(actionExecutedContext.Response.Headers.CacheControl);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Set_CacheControl_Header_When_HttpMethod_Is_Put()
            {
                var actionExecutedContext = GetHttpActionExecutedContext(
                    HttpMethod.Put);

                CreateSubjectUnderTest()
                    .OnActionExecuted(actionExecutedContext);

                Assert.IsNull(actionExecutedContext.Response.Headers.CacheControl);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Set_CacheControl_Header_When_HttpMethod_Is_Trace()
            {
                var actionExecutedContext = GetHttpActionExecutedContext(
                    HttpMethod.Trace);

                CreateSubjectUnderTest()
                    .OnActionExecuted(actionExecutedContext);

                Assert.IsNull(actionExecutedContext.Response.Headers.CacheControl);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Set_CacheControl_Header_When_Is_A_Successful_Get_Response_But_Its_Content_Is_Null()
            {
                var actionExecutedContext = GetHttpActionExecutedContext(
                    GetHttpResponseMessage(null));

                CreateSubjectUnderTest()
                    .OnActionExecuted(actionExecutedContext);

                Assert.IsNull(actionExecutedContext.Response.Headers.CacheControl);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Sets_CacheControl_Header_When_The_Response_Is_A_Successful_Get_Response_And_Has_Content()
            {
                var ClientTimeSpan = ObjectMother.Create<int>();
                var actionExecutedContext = GetHttpActionExecutedContext(
                    GetContent().ToArray());

                CreateSubjectUnderTest(ClientTimeSpan)
                    .OnActionExecuted(actionExecutedContext);

                Assert.IsNotNull(actionExecutedContext.Response.Headers.CacheControl);
                Assert.AreEqual(
                    TimeSpan.FromSeconds(ClientTimeSpan),
                    actionExecutedContext.Response.Headers.CacheControl.MaxAge);
            }          

            private static HttpActionExecutedContext GetHttpActionExecutedContext(
                byte[] content)
            {
                return GetHttpActionExecutedContext(
                    GetHttpResponseMessage(content));
            }

            private static HttpActionExecutedContext GetHttpActionExecutedContext(
                HttpResponseMessage response = null)
            {
                return GetHttpActionExecutedContext(HttpMethod.Get, response);
            }

            private static HttpActionExecutedContext GetHttpActionExecutedContext(
                HttpMethod method,
                HttpResponseMessage response = null)
            {
                var actionExecutedContext = new HttpActionExecutedContext(
                    GetHttpActionContext(method), null);

                actionExecutedContext.Response =
                    actionExecutedContext.ActionContext.Response =
                    response ?? GetHttpResponseMessage();

                return actionExecutedContext;
            }

            private static HttpResponseMessage GetHttpResponseMessage(
                byte[] content = null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);

                if (!content.IsNullOrEmpty())
                    response.Content = new ByteArrayContent(content);

                return response;
            }

            private static ClientOnlyOutputCacheAttribute CreateSubjectUnderTest(
                int clientTimeSpan = DefaultClientTimeSpan)
            {
                return new ClientOnlyOutputCacheAttribute(clientTimeSpan);
            }

            private static HttpActionContext GetHttpActionContext(
                HttpMethod method)
            {
                return new HttpActionContext(
                    GetHttpControllerContext(method),
                    GetHttpActionDescriptor());
            }

            private static HttpControllerContext GetHttpControllerContext(
                HttpMethod method)
            {
                var config = new HttpConfiguration();
                var request = GetHttpRequestMessage(method, config);

                return new HttpControllerContext(
                    config,
                    GetHttpRouteData(config, request),
                    request);
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

            private static HttpActionDescriptor GetHttpActionDescriptor()
            {
                return Mock.Of<HttpActionDescriptor>();
            }

            protected static bool AreEqual(
                byte[] expected,
                byte[] actual)
            {
                if (expected.Length != actual.Length)
                    return false;

                for (var i = 0; i < expected.Length; i++)
                    if (expected[i] != actual[i]) return false;

                return true;
            }

            protected static IEnumerable<byte> GetContent()
            {
                var size = GetRand(100);

                for (var i = 0; i < size; i++)
                    yield return ObjectMother.Create<byte>();
            }

            private static int GetRand(int max)
            {
                return new Random().Next(1, max);
            }
        }
    }
}
