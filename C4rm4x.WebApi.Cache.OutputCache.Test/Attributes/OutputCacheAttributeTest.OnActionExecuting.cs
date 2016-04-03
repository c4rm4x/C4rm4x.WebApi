#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache.Test
{
    public partial class OutputCacheAttributeTest
    {
        [TestClass]
        public class OutputCacheAttributeOnActionExecutingTest :
            OutputCacheAttributeFixture
        {
            [TestMethod, UnitTest]
            public void OnActionExecuting_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Delete()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuting(GetHttpActionContext(cache, HttpMethod.Delete));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Head()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuting(GetHttpActionContext(cache, HttpMethod.Head));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Options()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuting(GetHttpActionContext(cache, HttpMethod.Options));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Post()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuting(GetHttpActionContext(cache, HttpMethod.Post));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Put()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuting(GetHttpActionContext(cache, HttpMethod.Put));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Does_Not_Use_ICache_To_Retrieve_Content_When_HttpMethod_Is_Trace()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuting(GetHttpActionContext(cache, HttpMethod.Trace));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Never());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Uses_ICache_To_Retrieve_Content_When_HttpMethod_Is_Get()
            {
                var cache = GetCache();

                CreateSubjectUnderTest()
                    .OnActionExecuting(GetHttpActionContext(cache, HttpMethod.Get));

                Mock.Get(cache)
                    .Verify(c => c.Retrieve<byte[]>(It.IsAny<string>()), Times.Once());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Does_Not_Create_A_HttpResponseMessage_When_Cache_Does_Not_Have_Any_Item_With_Given_Key()
            {                
                var actionContext = GetHttpActionContext(null);

                CreateSubjectUnderTest()
                    .OnActionExecuting(actionContext);

                Assert.IsNull(actionContext.Response);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Creates_A_HttpResponseMessage_When_Cache_Has_An_Item_With_Given_Key()
            {
                var actionContext = GetHttpActionContext(GetContent().ToArray());

                CreateSubjectUnderTest()
                    .OnActionExecuting(actionContext);

                Assert.IsNotNull(actionContext.Response);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Creates_A_HttpResponseMessage_With_Content_As_Retrieved_From_Cache_When_This_Has_An_Item_With_Given_Key()
            {
                var content = GetContent().ToArray();
                var actionContext = GetHttpActionContext(content);

                CreateSubjectUnderTest()
                    .OnActionExecuting(actionContext);

                Assert.IsInstanceOfType(
                    actionContext.Response.Content, 
                    typeof(ByteArrayContent));

                var contentAsByteArray = actionContext.Response.Content as ByteArrayContent;
                Assert.IsTrue(
                    AreEqual(
                        content, 
                        contentAsByteArray.ReadAsByteArrayAsync().Result));
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Creates_A_HttpResponseMessage_With_Content_Header_ContentType_As_Application_Json_When_This_Has_An_Item_With_Given_Key()
            {
                var actionContext = GetHttpActionContext(GetContent().ToArray());

                CreateSubjectUnderTest()
                    .OnActionExecuting(actionContext);

                Assert.IsInstanceOfType(
                    actionContext.Response.Content,
                    typeof(ByteArrayContent));

                var contentAsByteArray = actionContext.Response.Content as ByteArrayContent;
                Assert.AreEqual(
                    "application/json", 
                    contentAsByteArray.Headers.ContentType.MediaType);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Creates_A_HttpResponseMessage_Without_CacheControl_Header_When_This_Has_An_Item_With_Given_Key_But_ClientTimeSpan_Is_Zero()
            {
                var actionContext = GetHttpActionContext(GetContent().ToArray());

                CreateSubjectUnderTest(clientTimeSpan: 0)
                    .OnActionExecuting(actionContext);

                Assert.IsNull(actionContext.Response.Headers.CacheControl);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Creates_A_HttpResponseMessage_With_CacheControl_Header_When_This_Has_An_Item_With_Given_Key_But_ClientTimeSpan_Is_Not_Zero()
            {
                var ClientTimeSpan = ObjectMother.Create<int>();
                var actionContext = GetHttpActionContext(GetContent().ToArray());

                CreateSubjectUnderTest(clientTimeSpan: ClientTimeSpan)
                    .OnActionExecuting(actionContext);

                Assert.IsNotNull(actionContext.Response.Headers.CacheControl);
                Assert.AreEqual(
                    TimeSpan.FromSeconds(ClientTimeSpan), 
                    actionContext.Response.Headers.CacheControl.MaxAge);
            }

            private static HttpActionContext GetHttpActionContext(
                byte[] content = null)
            {
                return GetHttpActionContext(GetCache(content));
            }

            private static ICache GetCache(byte[] content)
            {
                var cache = GetCache();

                Mock.Get(cache)
                    .Setup(c => c.Retrieve<byte[]>(Key))
                    .Returns(content);

                return cache;
            }
        }
    }
}
