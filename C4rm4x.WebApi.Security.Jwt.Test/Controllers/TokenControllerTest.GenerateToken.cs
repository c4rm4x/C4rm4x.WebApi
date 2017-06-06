#region Using

using C4rm4x.Tools.Security.Jwt;
using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Security.Jwt.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Test.Controllers
{
    public partial class TokenControllerTest
    {
        [TestClass]
        public class TokenControllerGenerateTokenTest : 
            AutoMockFixture<TokenController>
        {
            [TestInitialize]
            public override void Setup()
            {
                base.Setup();

                SetupHttpContext();

                Returns<IClaimsIdentityRetriever, Task<ClaimsIdentity>>(
                    r => r.RetrieveAsync(It.IsAny<string>(), It.IsAny<string>()), 
                    Task.FromResult(new ClaimsIdentity()));

                Returns<IJwtSecurityTokenGenerator, string>(
                    g => g.Generate(It.IsAny<ClaimsIdentity>(), It.IsAny<JwtGenerationOptions>()), 
                    ObjectMother.Create<string>());
            }

            [TestMethod, UnitTest]
            public void GenerateToken_Returns_BadRequest_Response_When_Request_UserIdentifier_Is_Null()
            {
                Assert.AreEqual(
                    HttpStatusCode.BadRequest,
                    GenerateToken(new GenerateTokenRequestBuilder().WithoutUserIdentifier().Build())
                        .Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void GenerateToken_Returns_BadRequest_Response_When_Request_UserIdentifier_Is_Empty_String()
            {
                Assert.AreEqual(
                    HttpStatusCode.BadRequest,
                    GenerateToken(new GenerateTokenRequestBuilder().WithUserIdentifier(string.Empty).Build())
                        .Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void GeneateToken_Uses_IClaimsIdentityRetriever_Retrieve_To_Retrieve_ClaimsIdentity_For_Specified_User()
            {
                var request = new GenerateTokenRequestBuilder().Build();
                var result = GenerateToken(request).Result;

                Verify<IClaimsIdentityRetriever>(
                    r => r.RetrieveAsync(request.UserIdentifier, request.Secret), Times.Once());
            }

            [TestMethod, UnitTest]
            public void GenerateToken_Returns_Unauthorized_Response_When_ClaimsIdentity_For_Specified_User_Cannot_Be_Found()
            {
                var request = new GenerateTokenRequestBuilder().Build();

                Returns<IClaimsIdentityRetriever, Task<ClaimsIdentity>>(
                    r => r.RetrieveAsync(request.UserIdentifier, request.Secret), 
                    Task.FromResult(null as ClaimsIdentity));

                Assert.AreEqual(
                    HttpStatusCode.Unauthorized,
                    GenerateToken(request).Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void GenerateToken_Uses_IJwtGenerationOptionsFactory_GetOptions_To_Retrieve_JwtGenerationOptions_For_Token_Generation_When_ClaimsIdentity_For_Specified_User_Is_Found()
            {
                var request = new GenerateTokenRequestBuilder().Build();
                var result = GenerateToken(request).Result;

                Verify<IJwtGenerationOptionsFactory>(
                    f => f.GetOptions(), Times.Once());
            }

            [TestMethod, UnitTest]
            public void GenerateToken_Uses_IJwtSecurityTokenGenerator_To_Generate_Token_When_ClaimsIdentity_For_Specified_User_Is_Found()
            {
                var request = new GenerateTokenRequestBuilder().Build();
                var claimsIdentity = new ClaimsIdentity();
                var options = It.IsAny<JwtGenerationOptions>();

                Returns<IJwtGenerationOptionsFactory, JwtGenerationOptions>(
                    f => f.GetOptions(), options);

                Returns<IClaimsIdentityRetriever, Task<ClaimsIdentity>>(
                    r => r.RetrieveAsync(request.UserIdentifier, request.Secret), 
                    Task.FromResult(claimsIdentity));

                var result = GenerateToken(request).Result;

                Verify<IJwtSecurityTokenGenerator>(
                    g => g.Generate(claimsIdentity, options), Times.Once());
            }

            [TestMethod, UnitTest]
            public void GenerateToken_Returns_Ok_Response_When_ClaimsIdentity_For_Specified_User_Is_Found()
            {
                Assert.AreEqual(
                    HttpStatusCode.OK,
                    GenerateToken(new GenerateTokenRequestBuilder().Build())
                        .Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void GenerateToken_Returns_GenerateTokenResponse_With_Token_Generated_When_ClaimsIdentity_For_Specified_User_Is_Found()
            {
                var Token = ObjectMother.Create<string>();

                Returns<IJwtSecurityTokenGenerator, string>(
                    g => g.Generate(It.IsAny<ClaimsIdentity>(), It.IsAny<JwtGenerationOptions>()),
                    Token);

                var result = GenerateToken(new GenerateTokenRequestBuilder().Build()).Result;
                var content = result.Content as ObjectContent<GenerateTokenResponse>;
                var response = content.Value as GenerateTokenResponse;

                Assert.AreEqual(Token, response.Token);
            }

            private Task<HttpResponseMessage> GenerateToken(GenerateTokenRequest generateTokenRequest)
            {
                return _sut.GenerateToken(generateTokenRequest)
                    .Result
                    .ExecuteAsync(It.IsAny<CancellationToken>());
            }

            private void SetupHttpContext()
            {
                _sut.Request = new HttpRequestMessage();
            }
        }
    }
}
