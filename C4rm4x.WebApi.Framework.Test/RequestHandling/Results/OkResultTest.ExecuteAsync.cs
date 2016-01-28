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
    public class OkResultTest
    {
        [TestClass]
        public class OkResultExecuteAsyncTest
        {
            #region Helper classes

            class TestResult { }

            #endregion

            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_Ok_Response()
            {
                Assert.AreEqual(
                    HttpStatusCode.OK,
                    ExecuteAsync().Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Sets_ContentType_Header_MediaType_As_ApplicationJson()
            {
                Assert.AreEqual(
                    "application/json",
                    ExecuteAsync().Result.Content.Headers.ContentType.MediaType);
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_Content_As_TContent()
            {
                Assert.IsInstanceOfType(
                    ExecuteAsync().Result.Content,
                    typeof(ObjectContent<TestResult>));
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Sets_ObjectContent_Value_As_Content()
            {
                var result = new TestResult();

                Assert.AreSame(
                    result,
                    (ExecuteAsync(result).Result.Content as ObjectContent<TestResult>).Value);
            }

            private static OkResult<TestResult> CreateSubjectUnderTest(
                TestResult result = null)
            {
                return new OkResult<TestResult>(
                    result ?? ObjectMother.Create<TestResult>());
            }

            private static Task<HttpResponseMessage> ExecuteAsync(
                TestResult result = null)
            {
                return CreateSubjectUnderTest(result)
                    .ExecuteAsync(It.IsAny<CancellationToken>());
            }
        }
    }
}
