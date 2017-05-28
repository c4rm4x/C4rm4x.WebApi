#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Test.RequestHandling.Results
{
    public partial class InternalServerErrorResultTest
    {
        [TestClass]
        public class InternalServerErrorResultExecuteAsyncTest
        {
            private const string TestApiCode = "Code";
            
            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_InternalServerError_Response()
            {
                Assert.AreEqual(
                    HttpStatusCode.InternalServerError,
                    ExecuteAsync<Exception>().Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Sets_ContentType_Header_MediaType_As_ApplicationJson()
            {
                Assert.AreEqual(
                    "application/json",
                    ExecuteAsync<Exception>().Result.Content.Headers.ContentType.MediaType);
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_Content_As_InternalServerError()
            {
                Assert.IsInstanceOfType(
                    ExecuteAsync<Exception>().Result.Content,
                    typeof(ObjectContent<InternalServerError>));
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_Content_As_InternalServerError_With_Code_As_UNKNONW_When_Exception_Does_Not_Inherit_From_ApiException()
            {
                var content = ExecuteAsync<Exception>().Result.Content;
                var value = content as ObjectContent<InternalServerError>;
                var internalServerError = value.Value as InternalServerError;

                Assert.AreEqual("UNKNOWN", internalServerError.Code);
            }

            private static InternalServerErrorResult CreateSubjectUnderTest<TException>(
                TException exception)
                where TException : Exception
            {
                return new InternalServerErrorResult<TException>(exception);
            }

            private static Task<HttpResponseMessage> ExecuteAsync<TException>()
                where TException : Exception, new()
            {
                return CreateSubjectUnderTest(new TException())
                    .ExecuteAsync(It.IsAny<CancellationToken>());
            }
        }
    }
}
