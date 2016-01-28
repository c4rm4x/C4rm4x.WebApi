#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Security.Test
{
    public partial class SecurityMessageHandlerTest
    {
        [TestClass]
        public class SecurityMessageHandlerSendAsyncTest
        {
            #region Helper classes

            class TestSecurityMessageHandler : SecurityMessageHandler
            {
                public bool IsAllowed { get; private set; }

                public TestSecurityMessageHandler(
                    bool isAllowed,
                    HttpResponseMessage responseMessage)
                {
                    IsAllowed = isAllowed;

                    InnerHandler = new TestHandler(responseMessage);
                }

                protected override bool IsRequestAllowed(HttpRequestMessage request)
                {
                    return IsAllowed;
                }
            }

            class TestHandler : DelegatingHandler
            {
                public HttpResponseMessage ResponseMessage { get; set; }

                public TestHandler(HttpResponseMessage responseMessage)
                {
                    ResponseMessage = responseMessage;
                }

                protected override Task<HttpResponseMessage> SendAsync(
                    HttpRequestMessage request,
                    CancellationToken cancellationToken)
                {
                    return Task.FromResult(ResponseMessage);
                }
            }

            #endregion

            [TestMethod, UnitTest]
            public void SendAsync_Returns_Forbiden_Response_When_Referrer_Is_Not_Allowed()
            {
                Assert.AreEqual(
                    HttpStatusCode.Forbidden,
                    SendAsync(false).Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_InnerHandler_Result_When_Referrer_Is_Allowed()
            {
                var Response = new HttpResponseMessage();

                Assert.AreSame(Response, SendAsync(true, Response).Result);
            }

            private Task<HttpResponseMessage> SendAsync(
                bool isValid = true,
                HttpResponseMessage response = null)
            {
                return new HttpMessageInvoker(
                    CreateSubjectUnderTest(isValid, response))
                    .SendAsync(Mock.Of<HttpRequestMessage>(), It.IsAny<CancellationToken>());
            }

            private TestSecurityMessageHandler CreateSubjectUnderTest(
                bool isValid,
                HttpResponseMessage response)
            {
                return new TestSecurityMessageHandler(isValid, response);
            }
        }
    }
}
