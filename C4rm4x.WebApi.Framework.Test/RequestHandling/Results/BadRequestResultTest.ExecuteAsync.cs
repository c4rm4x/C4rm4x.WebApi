#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.Test.RequestHandling.Results
{
    public class BadRequestResultTest
    {
        [TestClass]
        public class BadRequestResultExecuteAsyncTest
        {
            private const string ErrorMessage = "errorMessage";

            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_BadRequest_Response()
            {
                Assert.AreEqual(
                    HttpStatusCode.BadRequest,
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
            public void ExecuteAsync_Returns_Content_As_HttpError()
            {
                Assert.IsInstanceOfType(
                    ExecuteAsync().Result.Content,
                    typeof(ObjectContent<HttpError>));
            }

            private static BadRequestResult CreateSubjectUnderTest(
                string errorMessage = ErrorMessage)
            {
                return new BadRequestResult(
                    new ValidationException(errorMessage));
            }

            private static Task<HttpResponseMessage> ExecuteAsync(
                string errorMessage = ErrorMessage)
            {
                return CreateSubjectUnderTest(errorMessage)
                    .ExecuteAsync(It.IsAny<CancellationToken>());
            }
        }
    }
}
