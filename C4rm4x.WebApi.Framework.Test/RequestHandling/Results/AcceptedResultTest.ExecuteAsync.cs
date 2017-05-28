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
    public class AcceptedResultTest
    {
        [TestClass]
        public class AcceptedResultExecuteAsyncTest
        {
            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_Accepted_Response()
            {
                Assert.AreEqual(
                    HttpStatusCode.Accepted,
                    ExecuteAsync().Result.StatusCode);
            }            

            private static AcceptedResult CreateSubjectUnderTest()
            {
                return new AcceptedResult();
            }

            private static Task<HttpResponseMessage> ExecuteAsync()
            {
                return CreateSubjectUnderTest()
                    .ExecuteAsync(It.IsAny<CancellationToken>());
            }
        }
    }
}
