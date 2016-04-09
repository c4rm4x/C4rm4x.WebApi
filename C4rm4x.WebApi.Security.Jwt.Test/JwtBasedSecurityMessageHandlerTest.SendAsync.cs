#region Using

using C4rm4x.Tools.HttpUtilities;
using C4rm4x.Tools.Security.Jwt;
using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IdentityModel.Tokens;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Test
{
    public partial class JwtBasedSecurityMessageHandlerTest
    {
        [TestClass]
        public class JwtBasedSecurityMessageHandlerSendAsyncTest
        {
            [TestInitialize]
            public void Setup()
            {
                HttpContextFactory.SetCurrentContext(GetHttpContext());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_Unauthorized_Response_When_ForceAuthentication_Is_True_But_Authorization_Header_Is_Not_Present()
            {
                Assert.AreEqual(
                    HttpStatusCode.Unauthorized,
                    SendAsync(forceAuthentication: true, token: string.Empty).Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_InnerHandler_Result_When_ForceAuthentication_Is_False_And_Authorization_Header_Is_Not_Present()
            {
                var Response = new HttpResponseMessage();

                Assert.AreSame(
                    Response, 
                    SendAsync(forceAuthentication: false, token: string.Empty, response: Response).Result);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_Unauthorized_Response_When_Authorization_Header_Value_Is_Not_A_Valid_Jwt()
            {
                Assert.AreEqual(
                    HttpStatusCode.Unauthorized,
                    SendAsync(isValid: false).Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Returns_InnerHandler_Result_When_Authorization_Header_Value_Is_A_Valid_Jwt()
            {
                var Response = new HttpResponseMessage();

                Assert.AreSame(
                    Response,
                    SendAsync(isValid: true, response: Response).Result);
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_JwtSecurityTokenHandler_Validate_Twice_When_A_Valid_Jwt_Is_Found_In_Authorization_Header_Value()
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();
                var validatedToken = It.IsAny<SecurityToken>();

                SendAsync(tokenHandler);

                Mock.Get(tokenHandler)
                    .Verify(h => h.ValidateToken(It.IsAny<string>(), It.IsAny<TokenValidationParameters>(), out validatedToken),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_JwtSecurityTokenHandler_Validate_With_ValidateAudience_As_False()
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();
                var validatedToken = It.IsAny<SecurityToken>();

                SendAsync(tokenHandler, new JwtValidationOptions());

                Mock.Get(tokenHandler)
                    .Verify(h => h.ValidateToken(
                        It.IsAny<string>(),
                        It.Is<TokenValidationParameters>(p => !p.ValidateAudience),
                        out validatedToken),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_JwtSecurityTokenHandler_Validate_With_ValidateIssuer_As_False()
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();
                var validatedToken = It.IsAny<SecurityToken>();

                SendAsync(tokenHandler, new JwtValidationOptions());

                Mock.Get(tokenHandler)
                    .Verify(h => h.ValidateToken(
                        It.IsAny<string>(),
                        It.Is<TokenValidationParameters>(p => !p.ValidateIssuer),
                        out validatedToken),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_JwtSecurityTokenHandler_Validate_With_RequireExpirationTime_As_True_When_JwtValidationOptions_RequireExpirationTime_Is_True()
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();
                var validatedToken = It.IsAny<SecurityToken>();

                SendAsync(tokenHandler, new JwtValidationOptions(requireExpirationTime: true));

                Mock.Get(tokenHandler)
                    .Verify(h => h.ValidateToken(
                        It.IsAny<string>(), 
                        It.Is<TokenValidationParameters>(p => p.RequireExpirationTime), 
                        out validatedToken),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_JwtSecurityTokenHandler_Validate_With_ValidateLifetime_As_True_When_JwtValidationOptions_RequireExpirationTime_Is_True()
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();
                var validatedToken = It.IsAny<SecurityToken>();

                SendAsync(tokenHandler, new JwtValidationOptions(requireExpirationTime: true));

                Mock.Get(tokenHandler)
                    .Verify(h => h.ValidateToken(
                        It.IsAny<string>(),
                        It.Is<TokenValidationParameters>(p => p.ValidateLifetime),
                        out validatedToken),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_JwtSecurityTokenHandler_Validate_With_RequireExpirationTime_As_False_When_JwtValidationOptions_RequireExpirationTime_Is_False()
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();
                var validatedToken = It.IsAny<SecurityToken>();

                SendAsync(tokenHandler, new JwtValidationOptions(requireExpirationTime: false));

                Mock.Get(tokenHandler)
                    .Verify(h => h.ValidateToken(
                        It.IsAny<string>(),
                        It.Is<TokenValidationParameters>(p => !p.RequireExpirationTime),
                        out validatedToken),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_JwtSecurityTokenHandler_Validate_With_ValidateLifetime_As_False_When_JwtValidationOptions_RequireExpirationTime_Is_False()
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();
                var validatedToken = It.IsAny<SecurityToken>();

                SendAsync(tokenHandler, new JwtValidationOptions(requireExpirationTime: false));

                Mock.Get(tokenHandler)
                    .Verify(h => h.ValidateToken(
                        It.IsAny<string>(),
                        It.Is<TokenValidationParameters>(p => !p.ValidateLifetime),
                        out validatedToken),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_JwtSecurityTokenHandler_Validate_With_RequireSignedTokens_As_False_When_JwtValidationOptions_SigningKey_Is_Null()
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();
                var validatedToken = It.IsAny<SecurityToken>();

                SendAsync(tokenHandler, new JwtValidationOptions(signingKey: null));

                Mock.Get(tokenHandler)
                    .Verify(h => h.ValidateToken(
                        It.IsAny<string>(),
                        It.Is<TokenValidationParameters>(p => !p.RequireSignedTokens),
                        out validatedToken),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_JwtSecurityTokenHandler_Validate_With_IssuerSigningKey_As_Null_When_JwtValidationOptions_SigningKey_Is_Null()
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();
                var validatedToken = It.IsAny<SecurityToken>();

                SendAsync(tokenHandler, new JwtValidationOptions(signingKey: null));

                Mock.Get(tokenHandler)
                    .Verify(h => h.ValidateToken(
                        It.IsAny<string>(),
                        It.Is<TokenValidationParameters>(p => p.IssuerSigningKey.IsNull()),
                        out validatedToken),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_JwtSecurityTokenHandler_Validate_With_RequireSignedTokens_As_False_When_JwtValidationOptions_SigningKey_Is_Empty_String()
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();
                var validatedToken = It.IsAny<SecurityToken>();

                SendAsync(tokenHandler, new JwtValidationOptions(signingKey: string.Empty));

                Mock.Get(tokenHandler)
                    .Verify(h => h.ValidateToken(
                        It.IsAny<string>(),
                        It.Is<TokenValidationParameters>(p => !p.RequireSignedTokens),
                        out validatedToken),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_JwtSecurityTokenHandler_Validate_With_IssuerSigningKey_As_Null_When_JwtValidationOptions_SigningKey_Is_Empty_String()
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();
                var validatedToken = It.IsAny<SecurityToken>();

                SendAsync(tokenHandler, new JwtValidationOptions(signingKey: string.Empty));

                Mock.Get(tokenHandler)
                    .Verify(h => h.ValidateToken(
                        It.IsAny<string>(),
                        It.Is<TokenValidationParameters>(p => p.IssuerSigningKey.IsNull()),
                        out validatedToken),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_JwtSecurityTokenHandler_Validate_With_RequireSignedTokens_As_True_When_JwtValidationOptions_SigningKey_Is_Neither_Null_Nor_Empty_String()
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();
                var validatedToken = It.IsAny<SecurityToken>();

                SendAsync(tokenHandler, 
                    new JwtValidationOptions(
                        signingKey: Convert.ToBase64String(Encoding.UTF8.GetBytes(ObjectMother.Create<string>()))));

                Mock.Get(tokenHandler)
                    .Verify(h => h.ValidateToken(
                        It.IsAny<string>(),
                        It.Is<TokenValidationParameters>(p => p.RequireSignedTokens),
                        out validatedToken),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Uses_JwtSecurityTokenHandler_Validate_With_IssuerSigningKey_As_Instance_Of_InMemorySymmetricSecurityKey_When_JwtValidationOptions_SigningKey_Is_Neither_Null_Nor_Empty_String()
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();
                var validatedToken = It.IsAny<SecurityToken>();

                SendAsync(tokenHandler, 
                    new JwtValidationOptions(
                        signingKey: Convert.ToBase64String((Encoding.UTF8.GetBytes(ObjectMother.Create<string>())))));

                Mock.Get(tokenHandler)
                    .Verify(h => h.ValidateToken(
                        It.IsAny<string>(),
                        It.Is<TokenValidationParameters>(p => p.IssuerSigningKey is InMemorySymmetricSecurityKey),
                        out validatedToken),
                    Times.Once());
            }

            [TestMethod, UnitTest]
            public void SendAsync_Sets_Request_Context_Principal_When_A_Valid_Jwt_Is_Found_In_Authorization_Header_Value()
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();
                var validatedToken = It.IsAny<SecurityToken>();
                var Principal = Mock.Of<ClaimsPrincipal>();
                IPrincipal assignedPrincipal = null;

                Mock.Get(tokenHandler)
                    .Setup(h => h.ValidateToken(It.IsAny<string>(), It.IsAny<TokenValidationParameters>(), out validatedToken))
                    .Returns(Principal);

                SendAsync(tokenHandler, assignPrincipalAction: (r, p) => assignedPrincipal = p);

                Assert.AreSame(Principal, assignedPrincipal);
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

            private static HttpContextBase GetHttpContext()
            {
                return Mock.Of<HttpContextBase>();
            }

            private static Task<HttpResponseMessage> SendAsync(
                bool isValid = true,
                bool forceAuthentication = false,
                string token = null,
                HttpResponseMessage response = null)
            {
                return new HttpMessageInvoker(CreateSubjectUnderTest(isValid, forceAuthentication, response))
                    .SendAsync(
                        GetHttpRequestMessage(token ?? ObjectMother.Create<string>()), 
                        It.IsAny<CancellationToken>());
            }

            private static HttpRequestMessage GetHttpRequestMessage(string token)
            {
                var requestMessage = new HttpRequestMessage();

                if (!token.IsNullOrEmpty())
                    requestMessage.Headers.Add("Authorization", token);

                return requestMessage;
            }

            private static JwtBasedSecurityMessageHandler CreateSubjectUnderTest(
                bool isValid,
                bool forceAuthentication,
                HttpResponseMessage response)
            {
                return CreateSubjectUnderTest(
                    forceAuthentication,
                    response,
                    GetJwtSecurityTokenHandler(isValid));
            }

            private static JwtSecurityTokenHandler GetJwtSecurityTokenHandler(bool isValid)
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();
                var validatedToken = It.IsAny<SecurityToken>();

                if (!isValid)
                    Mock.Get(tokenHandler)
                        .Setup(h => h.ValidateToken(It.IsAny<string>(), It.IsAny<TokenValidationParameters>(), out validatedToken))
                        .Throws<SecurityTokenValidationException>();
                else
                    Mock.Get(tokenHandler)
                        .Setup(h => h.ValidateToken(It.IsAny<string>(), It.IsAny<TokenValidationParameters>(), out validatedToken))
                        .Returns(new ClaimsPrincipal());

                return tokenHandler;
            }

            private static JwtBasedSecurityMessageHandler CreateSubjectUnderTest(
                bool forceAuthentication,
                HttpResponseMessage response,
                JwtSecurityTokenHandler tokenHandler,
                JwtValidationOptions options = null,
                Action<HttpRequestMessage, IPrincipal> assignPrincipalAction = null)
            {
                IPrincipal principal;

                if (assignPrincipalAction.IsNull())
                    assignPrincipalAction = (r, p) => principal = p;

                var sut = new JwtBasedSecurityMessageHandler(
                    options ?? new JwtValidationOptions(), forceAuthentication);

                sut.InnerHandler = new TestHandler(response);
                sut.SetSecurityTokenHandlerFactory(() => tokenHandler);
                sut.SetAssignPrincipalFactory(assignPrincipalAction);

                return sut;
            }

            private static Task<HttpResponseMessage> SendAsync(
                JwtSecurityTokenHandler tokenHandler,
                JwtValidationOptions options = null,
                Action<HttpRequestMessage, IPrincipal> assignPrincipalAction = null)
            {
                return new HttpMessageInvoker(
                    CreateSubjectUnderTest(false, null, tokenHandler, options ?? new JwtValidationOptions(), assignPrincipalAction))
                    .SendAsync(
                        GetHttpRequestMessage(ObjectMother.Create<string>()),
                        It.IsAny<CancellationToken>());
            }
        }
    }
}
