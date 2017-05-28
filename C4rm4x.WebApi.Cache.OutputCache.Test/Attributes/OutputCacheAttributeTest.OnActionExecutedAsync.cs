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
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache.Test
{
    public partial class OutputCacheAttributeTest
    {
        [TestClass]
        public class OutputCacheAttributeOnActionExecutedAsyncTest :
            OutputCacheAttributeFixture
        {
            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Does_Not_Use_ICache_To_Retrieve_Content_When_ActionExecutedContext_Response_Is_Not_Successfull()
            {
                var cache = GetCache();

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(
                        GetHttpActionExecutedContext(
                            cache, 
                            new HttpResponseMessage(HttpStatusCode.InternalServerError)), 
                        It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.RetrieveAsync<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Delete()
            {
                var cache = GetCache();

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(
                        GetHttpActionExecutedContext(HttpMethod.Delete, cache),
                        It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.RetrieveAsync<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Head()
            {
                var cache = GetCache();

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(
                        GetHttpActionExecutedContext(HttpMethod.Head, cache), 
                        It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.RetrieveAsync<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Options()
            {
                var cache = GetCache();

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(
                        GetHttpActionExecutedContext(HttpMethod.Options, cache), 
                        It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.RetrieveAsync<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Post()
            {
                var cache = GetCache();

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(
                        GetHttpActionExecutedContext(HttpMethod.Post, cache), 
                        It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.RetrieveAsync<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Put()
            {
                var cache = GetCache();

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(
                        GetHttpActionExecutedContext(HttpMethod.Put, cache), 
                        It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.RetrieveAsync<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Trace()
            {
                var cache = GetCache();

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(
                        GetHttpActionExecutedContext(HttpMethod.Trace, cache), 
                        It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.RetrieveAsync<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Uses_ICache_To_Check_Whether_Or_Not_The_Content_Is_Already_Cached_When_HttpMethod_Is_Get()
            {
                var cache = GetCache();

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(
                        GetHttpActionExecutedContext(HttpMethod.Get, cache), 
                        It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.ExistsAsync(Key), Times.Once());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Does_Not_Use_ICache_To_Store_Content_When_The_Content_Is_Already_Cached()
            {
                var cache = GetCache();

                Mock.Get(cache)
                    .Setup(c => c.ExistsAsync(Key))
                    .Returns(Task.FromResult(true));

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(
                        GetHttpActionExecutedContext(cache), 
                        It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.StoreAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<int>()), 
                    Times.Never());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Does_Not_Use_ICache_To_Store_Content_When_The_ActionExecutedContext_Response_Content_Is_Null()
            {
                var cache = GetCache();

                await CreateSubjectUnderTest()
                    .OnActionExecutedAsync(
                        GetHttpActionExecutedContext(null, cache), 
                        It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.StoreAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<int>()),
                    Times.Never());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Uses_ICache_To_Store_Content_When_The_ActionExecutedContext_Response_Content_Is_Not_Null()
            {
                var cache = GetCache();                
                var content = GetContent().ToArray();
                var ServerTimeSpan = ObjectMother.Create<int>();

                await CreateSubjectUnderTest(serverTimeSpan: ServerTimeSpan)
                    .OnActionExecutedAsync(
                        GetHttpActionExecutedContext(content, cache),
                        It.IsAny<CancellationToken>());

                Mock.Get(cache)
                    .Verify(c => c.StoreAsync(Key, content, ServerTimeSpan),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Creates_A_HttpResponseMessage_Without_CacheControl_Header_When_This_Has_An_Item_With_Given_Key_But_ClientTimeSpan_Is_Zero()
            {
                var actionExecutedContext = GetHttpActionExecutedContext(GetContent().ToArray());

                await CreateSubjectUnderTest(clientTimeSpan: 0)
                    .OnActionExecutedAsync(actionExecutedContext, It.IsAny<CancellationToken>());

                Assert.IsNull(actionExecutedContext.Response.Headers.CacheControl);
            }

            [TestMethod, UnitTest]
            public async Task OnActionExecutedAsync_Creates_A_HttpResponseMessage_With_CacheControl_Header_When_This_Has_An_Item_With_Given_Key_But_ClientTimeSpan_Is_Not_Zero()
            {
                var ClientTimeSpan = ObjectMother.Create<int>();
                var actionExecutedContext = GetHttpActionExecutedContext(GetContent().ToArray());

                await CreateSubjectUnderTest(clientTimeSpan: ClientTimeSpan)
                    .OnActionExecutedAsync(actionExecutedContext, It.IsAny<CancellationToken>());

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
