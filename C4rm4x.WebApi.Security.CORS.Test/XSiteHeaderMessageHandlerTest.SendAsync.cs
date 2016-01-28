#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Security.CORS.Test
{
    public partial class XSiteHeaderMessageHandlerTest
    {
        [TestClass]
        public class XSiteHeaderMessageHandlerSendAsyncTest
            : AutoMockFixture<XSiteHeaderMessageHandler>
        {
            [TestInitialize]
            public override void Setup()
            {
                base.Setup();

                _sut.InnerHandler = Mock.Of<DelegatingHandler>();
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_Forbiden_Response_When_Referrer_Is_Not_Allowed()
            {
                Assert.AreEqual(
                    HttpStatusCode.Forbidden,
                    SendAsync(false).Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Adds_Response_Header_When_Referrer_Is_Allowed()
            {
                var Request = new HttpRequestMessage();

                SendAsync(true, Request);

                Verify<IXSiteHeaderService>(s =>
                    s.AddResponseHeader(Request, It.IsAny<HttpResponseMessage>()), Times.Once());
            }

            private Task<HttpResponseMessage> SendAsync(
                bool isValid = true,
                HttpRequestMessage request = null)
            {
                request = request ?? Mock.Of<HttpRequestMessage>();

                Returns<IXSiteHeaderService, bool>(s =>
                    s.IsReferrerAllowed(request), isValid);

                return new HttpMessageInvoker(_sut)
                    .SendAsync(request, It.IsAny<CancellationToken>());
            }
        }
    }
}