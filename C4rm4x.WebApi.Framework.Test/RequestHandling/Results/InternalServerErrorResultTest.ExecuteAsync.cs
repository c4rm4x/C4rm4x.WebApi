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
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.Test.RequestHandling.Results
{
    public partial class InternalServerErrorResultTest
    {
        [TestClass]
        public class InternalServerErrorResultExecuteAsyncTest
        {
            private const string TestApiCode = "Code";
            
            #region Helper classes
            
            private class TestApiException : ApiException
            {
                public TestApiException(string message)
                    : base(TestApiCode, message)
                {}
            }
            
            #endregion
            
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
            public void ExecuteAsync_Returns_Content_As_HttpError_When_Exception_Does_Not_Inherit_From_ApiException()
            {
                Assert.IsInstanceOfType(
                    ExecuteAsync<Exception>().Result.Content,
                    typeof(ObjectContent<HttpError>));
            }
            
            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_Content_As_Anonymous_Object_With_Code_And_Description_When_Exception_Inherits_From_ApiException()
            {
                var Message = ObjectMother.Create(100);
                
                var content = ExecuteAsync<TestApiException>(Message).Result.Content;
                
                dynamic error = (content as ObjectContent).Value;
                Assert.IsNotNull(error);
                Assert.AreEqual(TestApiCode, error.Code);
                Assert.AreEqual(Message, error.Message);
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
