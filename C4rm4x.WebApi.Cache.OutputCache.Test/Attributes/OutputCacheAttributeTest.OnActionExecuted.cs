#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
                    .OnActionExecuted(GetHttpActionExecutedContext(cache, HttpMethod.Delete));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Head()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(cache, HttpMethod.Head));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Options()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(cache, HttpMethod.Options));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Post()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(cache, HttpMethod.Post));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Put()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(cache, HttpMethod.Put));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Trace()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(cache, HttpMethod.Trace));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuted_Uses_ICache_To_Retrieve_Content_When_HttpMethod_Is_Get()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuted(GetHttpActionExecutedContext(cache, HttpMethod.Get));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Once());
            }

            private static HttpActionExecutedContext GetHttpActionExecutedContext(
                ICache cache,
                HttpResponseMessage response)
            {
                return GetHttpActionExecutedContext(cache, HttpMethod.Get, response);
            }

            private static HttpActionExecutedContext GetHttpActionExecutedContext(
                ICache cache,
                HttpMethod method,
                HttpResponseMessage response = null)
            {
                var actionExecutedContext = new HttpActionExecutedContext(
                    GetHttpActionContext(cache, method), null);

                actionExecutedContext.Response = 
                    actionExecutedContext.ActionContext.Response =
                    response ?? new HttpResponseMessage(HttpStatusCode.OK);

                return actionExecutedContext;
            }
        }
    }
}
