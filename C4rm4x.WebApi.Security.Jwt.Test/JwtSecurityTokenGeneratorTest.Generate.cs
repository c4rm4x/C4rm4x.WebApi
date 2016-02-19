#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Test
{
    public partial class JwtSecurityTokenGeneratorTest
    {
        [TestClass]
        public class JwtSecurityTokenGeneratorGenerateTest
        {
            private const string Issuer = "default";

            [TestMethod, UnitTest]
            public void Generate_Uses_JwtSecurityTokenHandler_Generate_Subject_As_Subject_Argument()
            {
                var Subject = new ClaimsIdentity();
                var tokenHandler = GetSecurityTokenHandler();

                CreateSubjectUnderTest(tokenHandler)
                    .Generate(Subject, new JwtGenerationOptions());

                Mock.Get(tokenHandler)
                    .Verify(t => t.CreateToken(
                        Issuer, null, Subject, It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<SigningCredentials>(), null),
                        Times.Once());
            }

            [TestMethod, UnitTest]
            public void Generate_Uses_JwtSecurityTokenHandler_Generate_With_DateTime_Now_As_NotBefore_Argument()
            {
                var Now = DateTime.UtcNow;
                var tokenHandler = GetSecurityTokenHandler();

                CreateSubjectUnderTest(tokenHandler, Now)
                    .Generate(null, new JwtGenerationOptions());

                Mock.Get(tokenHandler)
                    .Verify(t => t.CreateToken(
                        Issuer, null, It.IsAny<ClaimsIdentity>(), Now, It.IsAny<DateTime?>(), It.IsAny<SigningCredentials>(), null),
                        Times.Once());
            }

            [TestMethod, UnitTest]
            public void Generate_Uses_JwtSecurityTokenHandler_Generate_With_DateTime_Now_Plus_Options_TokenLifetimeInMinutes_As_Expires_Argument()
            {
                var Now = DateTime.UtcNow;
                var TokenLifetimeInMinutes = ObjectMother.Create<double>();
                var tokenHandler = GetSecurityTokenHandler();

                CreateSubjectUnderTest(tokenHandler, Now)
                    .Generate(null, new JwtGenerationOptions(tokenLifetimeInMinutes: TokenLifetimeInMinutes));

                Mock.Get(tokenHandler)
                    .Verify(t => t.CreateToken(
                        Issuer, null, It.IsAny<ClaimsIdentity>(), It.IsAny<DateTime?>(), It.Is<DateTime?>(d => d == Now.AddMinutes(TokenLifetimeInMinutes)), It.IsAny<SigningCredentials>(), null),
                        Times.Once());
            }

            [TestMethod, UnitTest]
            public void Generate_Uses_JwtSecurityTokenHandler_Generate_With_Null_As_SigningCredentials_Argument_When_Options_SigningCredentials_Is_Null()
            {
                var tokenHandler = GetSecurityTokenHandler();

                CreateSubjectUnderTest(tokenHandler)
                    .Generate(null, new JwtGenerationOptions(signingCredentials: null));

                Mock.Get(tokenHandler)
                    .Verify(t => t.CreateToken(
                        Issuer, null, It.IsAny<ClaimsIdentity>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), null, null),
                        Times.Once());
            }

            [TestMethod, UnitTest]
            public void Generate_Uses_JwtSecurityTokenHandler_Generate_With_Null_As_SigningCredentials_Argument_When_Options_SigningCredentials_Is_Not_Null_But_SigningAlgorithm_Is_JwtSigningAlgorithm_NONE()
            {
                var tokenHandler = GetSecurityTokenHandler();

                CreateSubjectUnderTest(tokenHandler)
                    .Generate(null, new JwtGenerationOptions(
                        signingCredentials: new JwtSigningCredentials(algorithm: JwtSigningAlgorithm.NONE)));

                Mock.Get(tokenHandler)
                    .Verify(t => t.CreateToken(
                        Issuer, null, It.IsAny<ClaimsIdentity>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), null, null),
                        Times.Once());
            }

            [TestMethod, UnitTest]
            public void Generate_Uses_JwtSecurityTokenHandler_Generate_With_Instance_Of_HmacSigningCredentials_As_SigningCredentials_Argument_When_Options_SigningCredentials_Is_Not_Null_And_SigningAlgorithm_Is_JwtSigningAlgorithm_HMAC_SHA256()
            {
                var tokenHandler = GetSecurityTokenHandler();

                CreateSubjectUnderTest(tokenHandler)
                    .Generate(null, new JwtGenerationOptions(
                        signingCredentials: new JwtSigningCredentials(
                            algorithm: JwtSigningAlgorithm.HMAC_SHA256,
                            key: Convert.ToBase64String(Encoding.UTF8.GetBytes(ObjectMother.Create(64))))));

                Mock.Get(tokenHandler)
                    .Verify(t => t.CreateToken(
                        Issuer, null, It.IsAny<ClaimsIdentity>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.Is<SigningCredentials>(s => s is HmacSigningCredentials), null),
                        Times.Once());
            }

            [TestMethod, UnitTest]
            public void Generate_Uses_JwtSecurityTokenHandler_WriteToken_With_Instance_Of_JwtSecurityToken_Created()
            {
                var SecurityToken = It.IsAny<JwtSecurityToken>();
                var tokenHandler = GetSecurityTokenHandler(securityToken: SecurityToken);

                CreateSubjectUnderTest(tokenHandler)
                    .Generate(null, new JwtGenerationOptions());

                Mock.Get(tokenHandler)
                    .Verify(h => h.WriteToken(SecurityToken), Times.Once());
            }

            [TestMethod, UnitTest]
            public void Generate_Returns_Token_Generated_By_JwtSecurityTokenHandler_WriteToken()
            {
                var Token = ObjectMother.Create<string>();

                Assert.AreEqual(
                    Token,
                    CreateSubjectUnderTest(GetSecurityTokenHandler(token: Token))
                        .Generate(null, new JwtGenerationOptions()));
            }

            private static JwtSecurityTokenGenerator CreateSubjectUnderTest(
                JwtSecurityTokenHandler tokenHandler,
                DateTime? now = null)
            {
                var sut = new JwtSecurityTokenGenerator();

                sut.SetDateTimeNowFactory(() => now ?? DateTime.Now);
                sut.SetSecurityTokenHandlerFactory(() => tokenHandler);

                return sut;
            }

            private static JwtSecurityTokenHandler GetSecurityTokenHandler(
                string token = null,
                JwtSecurityToken securityToken = null)
            {
                var tokenHandler = Mock.Of<JwtSecurityTokenHandler>();

                Mock.Get(tokenHandler)
                    .Setup(h => h.CreateToken(
                        Issuer,
                        null,
                        It.IsAny<ClaimsIdentity>(),
                        It.IsAny<DateTime?>(),
                        It.IsAny<DateTime?>(),
                        It.IsAny<SigningCredentials>(),
                        null))
                    .Returns(securityToken = securityToken ?? It.IsAny<JwtSecurityToken>());

                Mock.Get(tokenHandler)
                    .Setup(h => h.WriteToken(securityToken))
                    .Returns(token ?? ObjectMother.Create<string>());

                return tokenHandler;
            }
        }
    }
}
