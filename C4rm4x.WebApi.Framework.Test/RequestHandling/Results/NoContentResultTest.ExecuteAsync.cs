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
    public class NoContentResultTest
    {
        [TestClass]
        public class NoContentResultExecuteAsyncTest
        {
            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_NoContent_Response()
            {
                Assert.AreEqual(
                    HttpStatusCode.NoContent,
                    ExecuteAsync().Result.StatusCode);
            }            

            private static NoContentResult CreateSubjectUnderTest()
            {
                return new NoContentResult();
            }

            private static Task<HttpResponseMessage> ExecuteAsync()
            {
                return CreateSubjectUnderTest()
                    .ExecuteAsync(It.IsAny<CancellationToken>());
            }
        }
    }
}
