using C4rm4x.Tools.HttpUtilities;
using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Controllers;

namespace C4rm4x.WebApi.Security.Acl.Test
{
    public partial class DigitalSignedAttributeTest
    {
        [TestClass]
        public class DigitalSignedAttributeOnActionExecutingTest
        {
            private const string SharedSecretClaimType = "Claim.Shared.Secret";
            
            [TestInitialize]
            public void Setup()
            {
                HttpContextFactory.SetCurrentContext(GetHttpContext());
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Sets_Response_As_BadRequest_When_Signature_Header_Is_Not_Present()
            {
                var actionContext = GetActionContext(header: null);

                OnActionExecuting(actionContext);

                Assert.AreEqual(HttpStatusCode.BadRequest, actionContext.Response.StatusCode);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Sets_Response_As_BadRequest_When_Principal_Does_Not_Include_Shared_Secret_Claim()
            {
                var actionContext = GetActionContext(
                    header: ObjectMother.Create<string>(), sharedSecret: null);

                OnActionExecuting(actionContext);

                Assert.AreEqual(HttpStatusCode.BadRequest, actionContext.Response.StatusCode);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Sets_Response_As_BadRequest_When_Principal_Does_Include_Shared_Secret_Claim_But_An_Invalid_Signature_Header_Is_Present()
            {
                var actionContext = GetActionContext(
                    header: ObjectMother.Create<string>(), 
                    sharedSecret: ObjectMother.Create<string>());

                OnActionExecuting(actionContext, signature: ObjectMother.Create<string>());

                Assert.AreEqual(HttpStatusCode.BadRequest, actionContext.Response.StatusCode);
            }

            [TestMethod, UnitTest]
            public void OnActionExecuting_Does_Not_Set_Response_When_Principal_Does_Include_Shared_Secret_Claim_And_A_Valid_Signature_Header_Is_Present()
            {
                var signature = ObjectMother.Create<string>();

                var actionContext = GetActionContext(
                    header: signature, sharedSecret: ObjectMother.Create<string>());

                OnActionExecuting(actionContext, signature: signature);

                Assert.IsNull(actionContext.Response);
            }

            private void OnActionExecuting(HttpActionContext actionContext, string signature = null)
            {
                CreateSubjectUnderTest(signature).OnActionExecuting(actionContext);
            }

            private static DigitalSignedAttribute CreateSubjectUnderTest(string signature = null)
            {
                var attribute = new TestDigitalSignedAttribute();

                attribute.SetSigner(
                    (body, secret) => signature ?? ObjectMother.Create<string>());

                return attribute;
            }

            private static HttpActionContext GetActionContext(
                string sharedSecret = null,
                string header = null)
            {
                return new HttpActionContext(
                    GetControllerContext(sharedSecret, header),
                    Mock.Of<HttpActionDescriptor>());
            }

            private static HttpControllerContext GetControllerContext(
                string sharedSecret = null,
                string header = null)
            {
                return new HttpControllerContext(
                    GetRequestContext(sharedSecret),
                    GetHttpRequestMessage(header),
                    Mock.Of<HttpControllerDescriptor>(),
                    Mock.Of<IHttpController>());
            }

            private static HttpRequestContext GetRequestContext(
                string sharedSecret = null)
            {
                var requestContext = Mock.Of<HttpRequestContext>();

                Mock.Get(requestContext)
                    .SetupGet(r => r.Principal)
                    .Returns(GetPrincipal(sharedSecret));

                return requestContext;
            }

            private static IPrincipal GetPrincipal(string sharedSecret = null)
            {
                var principal = Mock.Of<ClaimsPrincipal>();

                Mock.Get(principal)
                    .SetupGet(p => p.Identity)
                    .Returns(Mock.Of<IIdentity>());

                Mock.Get(principal)
                    .Setup(p => p.FindFirst(It.IsAny<Predicate<Claim>>()))
                    .Returns(sharedSecret.IsNullOrEmpty() ? null : new Claim(SharedSecretClaimType, sharedSecret));

                return principal;
            }

            private static HttpRequestMessage GetHttpRequestMessage(string header = null)
            {
                var requestMessage = new HttpRequestMessage();

                if (!header.IsNullOrEmpty())
                    requestMessage.Headers.Add("X-BodyDigitalSignature", header);

                return requestMessage;
            }

            private static HttpContextBase GetHttpContext()
            {
                var context = Mock.Of<HttpContextBase>();

                Mock.Get(context)
                    .SetupGet(c => c.Request)
                    .Returns(GetHttpRequest());

                return context;
            }

            private static HttpRequestBase GetHttpRequest()
            {
                var request = Mock.Of<HttpRequestBase>();

                Mock.Get(request)
                    .SetupGet(r => r.InputStream)
                    .Returns(new MemoryStream(new byte[1024]));

                return request;
            }

            #region Helper classes

            private class TestDigitalSignedAttribute : DigitalSignedAttribute
            {
                public TestDigitalSignedAttribute() :
                    base(SharedSecretClaimType)
                {

                }
            }

            #endregion
        }
    }
}
