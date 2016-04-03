#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache.Test
{
    public partial class OutputCacheAttributeTest
    {
        [TestClass]
        public class OutputCacheAttributeOnActionExecutedTest :
            OutputCacheAttributeFixture
        {
            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Use_ICache_To_Retrieve_Content_When_ActionExecutedContext_Response_Is_Not_Successfull()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuted(
                        GetHttpActionExecutedContext(
                            cache, 
                            new HttpResponseMessage(HttpStatusCode.InternalServerError)));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Delete()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(HttpMethod.Delete, cache));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Head()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(HttpMethod.Head, cache));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Options()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(HttpMethod.Options, cache));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Post()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(HttpMethod.Post, cache));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Put()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(HttpMethod.Put, cache));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Trace()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(HttpMethod.Trace, cache));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Uses_ICache_To_Check_Whether_Or_Not_The_Content_Is_Already_Cached_When_HttpMethod_Is_Get()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(HttpMethod.Get, cache));

                Mock.Get(cache)
                    .Verify(c => c.Exists(Key), Times.Once());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Use_ICache_To_Store_Content_When_The_Content_Is_Already_Cached()
            {
                var cache = GetCache();

                Mock.Get(cache)
                    .Setup(c => c.Exists(Key))
                    .Returns(true);

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(cache));

                Mock.Get(cache)
                    .Verify(c => c.Store(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<int>()), 
                    Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Use_ICache_To_Store_Content_When_The_ActionExecutedContext_Response_Content_Is_Null()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(null, cache));

                Mock.Get(cache)
                    .Verify(c => c.Store(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<int>()),
                    Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Uses_ICache_To_Store_Content_When_The_ActionExecutedContext_Response_Content_Is_Not_Null()
            {
                var cache = GetCache();                
                var content = GetContent().ToArray();
                var ServerTimeSpan = ObjectMother.Create<int>();

                CreateSubjectUnderTest(serverTimeSpan: ServerTimeSpan)
                    .OnActionExecuted(GetHttpActionExecutedContext(content, cache));

                Mock.Get(cache)
                    .Verify(c => c.Store(Key, content, ServerTimeSpan),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Creates_A_HttpResponseMessage_Without_CacheControl_Header_When_This_Has_An_Item_With_Given_Key_But_ClientTimeSpan_Is_Zero()
            {
                var actionExecutedContext = GetHttpActionExecutedContext(GetContent().ToArray());

                CreateSubjectUnderTest(clientTimeSpan: 0)
                    .OnActionExecuted(actionExecutedContext);

                Assert.IsNull(actionExecutedContext.Response.Headers.CacheControl);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Creates_A_HttpResponseMessage_With_CacheControl_Header_When_This_Has_An_Item_With_Given_Key_But_ClientTimeSpan_Is_Not_Zero()
            {
                var ClientTimeSpan = ObjectMother.Create<int>();
                var actionExecutedContext = GetHttpActionExecutedContext(GetContent().ToArray());

                CreateSubjectUnderTest(clientTimeSpan: ClientTimeSpan)
                    .OnActionExecuted(actionExecutedContext);

                Assert.IsNotNull(actionExecutedContext.Response.Headers.CacheControl);
                Assert.AreEqual(
                    TimeSpan.FromSeconds(ClientTimeSpan),
                    actionExecutedContext.Response.Headers.CacheControl.MaxAge);
            }

            private static HttpActionExecutedContext GetHttpActionExecutedContext(
                byte[] content, 
                ICache cache = null)
            {
                return GetHttpActionExecutedContext(
                    cache ?? GetCache(), 
                    GetHttpResponseMessage(content));
            }

            private static HttpActionExecutedContext GetHttpActionExecutedContext(
                ICache cache,
                HttpResponseMessage response = null)
            {
                return GetHttpActionExecutedContext(HttpMethod.Get, cache, response);
            }

            private static HttpActionExecutedContext GetHttpActionExecutedContext(
                HttpMethod method,
                ICache cache,                
                HttpResponseMessage response = null)
            {
                var actionExecutedContext = new HttpActionExecutedContext(
                    GetHttpActionContext(cache, method), null);

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
        }
    }
}
