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
    public class PaymentRequiredResultTest
    {
        [TestClass]
        public class PaymentRequiredResultExecuteAsyncTest
        {
            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_PaymentRequired_Response()
            {
                Assert.AreEqual(
                    HttpStatusCode.PaymentRequired,
                    ExecuteAsync().Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_Content_As_StringContent()
            {
                Assert.IsInstanceOfType(
                    ExecuteAsync().Result.Content,
                    typeof(StringContent));
            }

            private static PaymentRequiredResult CreateSubjectUnderTest(string reason = null)
            {
                return new PaymentRequiredResult(reason ?? ObjectMother.Create<string>());
            }

            private static Task<HttpResponseMessage> ExecuteAsync(string reason = null)
            {
                return CreateSubjectUnderTest(reason)
                    .ExecuteAsync(It.IsAny<CancellationToken>());
            }
        }
    }
}
