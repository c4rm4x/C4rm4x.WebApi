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
    public partial class UnprocessableEntityResultTest
    {
        [TestClass]
        public class UnprocessableEntityResultExecuteAsyncTest
        {
            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_UnprocessableEntity_Response()
            {
                Assert.AreEqual(
                    (HttpStatusCode)422,
                    ExecuteAsync().Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_Content_As_StringContent()
            {
                Assert.IsInstanceOfType(
                    ExecuteAsync().Result.Content,
                    typeof(StringContent));
            }

            private static UnprocessableEntityResult CreateSubjectUnderTest(string reason = null)
            {
                return new UnprocessableEntityResult(reason ?? ObjectMother.Create<string>());
            }

            private static Task<HttpResponseMessage> ExecuteAsync(string reason = null)
            {
                return CreateSubjectUnderTest(reason)
                    .ExecuteAsync(It.IsAny<CancellationToken>());
            }
        }
    }
}
