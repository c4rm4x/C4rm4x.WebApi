#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;

#endregion

namespace C4rm4x.WebApi.Security.Cors.Test
{
    public partial class CorsBasedSecurityMessageHandlerTest
    {
        [TestClass]
        public class CorsBasedSecurityMessageHandlerSendAsyncTest
        {
            private const string Origin = "Origin";

            [TestMethod, UnitTest]
            public void SendAsync_Returns_InnerHandler_Result_When_Origin_Header_Is_Not_Present()
            {
                var Response = new HttpResponseMessage();

                Assert.AreSame(
                    Response,
                    SendAsync(origin: null, response: Response).Result);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_Forbiden_Response_When_Origin_Header_Is_Present_But_CORS_Policy_Is_Not_Met()
            {
                Assert.AreEqual(
                    HttpStatusCode.Forbidden,
                    SendAsync(isCorsValid: false).Result.StatusCode);
            }

            private static Task<HttpResponseMessage> SendAsync(
                bool isCorsValid = true)
            {
                return new HttpMessageInvoker(CreateSubjectUnderTest(isCorsValid))
                    .SendAsync(
                        GetHttpRequestMessage(Origin),
                        It.IsAny<CancellationToken>());
            }

            private static CorsBasedSecurityMessageHandler CreateSubjectUnderTest(
                bool isCorsValid,
                HttpResponseMessage response = null)
            {
                return CreateSubjectUnderTest(null, GetCorsEngine(isCorsValid));
            }

            private static CorsEngine GetCorsEngine(bool isCorsValid)
            {
                var corsEngine = Mock.Of<CorsEngine>();

                Mock.Get(corsEngine)
                    .Setup(e => e.EvaluatePolicy(It.IsAny<CorsRequestContext>(), It.IsAny<CorsPolicy>()))
                    .Returns(GetResult(isCorsValid));

                return corsEngine;
            }

            private static CorsResult GetResult(bool isCorsValid)
            {
                return isCorsValid
                    ? new CorsResult()
                    : null;
            }

            private static Task<HttpResponseMessage> SendAsync(
                string origin = Origin,
                HttpResponseMessage response = null)
            {
                return new HttpMessageInvoker(CreateSubjectUnderTest(response))
                    .SendAsync(
                        GetHttpRequestMessage(origin),
                        It.IsAny<CancellationToken>());
            }

            private static CorsBasedSecurityMessageHandler CreateSubjectUnderTest(
                HttpResponseMessage response,
                CorsEngine corsEngine  = null,
                CorsPolicy policy = null)
            {
                var sut = new CorsBasedSecurityMessageHandler(policy ?? new CorsPolicy());

                sut.InnerHandler = new TestHandler(response);
                sut.SetCorsEngineFactory(() => corsEngine ?? new CorsEngine());

                return sut;
            }

            private static HttpRequestMessage GetHttpRequestMessage(string origin)
            {
                var requestMessage = new HttpRequestMessage();

                if (!origin.IsNullOrEmpty())
                    requestMessage.Headers.Add(CorsConstants.Origin, origin);

                return requestMessage;
            }

            #region Helper classes

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
        }
    }
}
