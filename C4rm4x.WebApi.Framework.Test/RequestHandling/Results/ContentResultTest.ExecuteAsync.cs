#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Test.RequestHandling.Results
{
    public class ContentResultTest
    {
        [TestClass]
        public class ContentResultExecuteAsyncTest
        {
            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_Ok_Response()
            {
                Assert.AreEqual(
                    HttpStatusCode.OK,
                    ExecuteAsync().Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Sets_ContentType_Header_MediaType_As_Passed_As_Argument()
            {
                Assert.AreEqual(
                    "video/avi",
                    ExecuteAsync(mimeType: "video/avi").Result.Content.Headers.ContentType.MediaType);
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_Content_As_ByteArrayContent()
            {
                var result = (ExecuteAsync().Result.Content
                     as ObjectContent<ByteArrayContent>).Value;

                Assert.IsInstanceOfType(
                    result,
                    typeof(ByteArrayContent));
            }

            private static ContentResult CreateSubjectUnderTest(
                byte[] content = null,
                string mimeType = null)
            {
                return new ContentResult(
                    content ?? new byte[1024],
                    mimeType ?? "image/png");
            }

            private static Task<HttpResponseMessage> ExecuteAsync(
                byte[] content = null,
                string mimeType = null)
            {
                return CreateSubjectUnderTest(content, mimeType)
                    .ExecuteAsync(It.IsAny<CancellationToken>());
            }
        }
    }
}
