#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IdentityModel;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Principal;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Test
{
    public partial class JwtSecurityTokenGeneratorTest
    {
        [TestClass]
        public class JwtSecurityTokenGeneratorIntegrationTest
        {
            [TestMethod, IntegrationTest]
            public void Validation_Fails_When_Token_Is_Not_Signed_But_Validation_Requires_Signature()
            {
                var token = GetTokenGenerator().Generate(null, new JwtGenerationOptions(signingCredentials: null));

                Assert.IsFalse(TryValidateToken(token, new JwtValidationOptions(signingKey: GetRandomKey())));
            }

            [TestMethod, IntegrationTest]
            [ExpectedException(typeof(SignatureVerificationFailedException))]
            public void Validation_Throws_SignatureVerificationFailedException_When_Token_Is_Signed_But_Validation_Does_Not_Require_Signature()
            {
                var token = GetTokenGenerator().Generate(null, new JwtGenerationOptions(
                    signingCredentials: new JwtSigningCredentials(JwtSigningAlgorithm.HMAC_SHA256, GetRandomKey())));

                TryValidateToken(token, new JwtValidationOptions());
            }

            [TestMethod, IntegrationTest]
            public void Validation_Does_Not_Fail_When_Token_Is_Not_Signed_And_Validation_Does_Not_Require_Signature()
            {
                var key = GetRandomKey();
                var token = GetTokenGenerator().Generate(null, new JwtGenerationOptions(
                    signingCredentials: new JwtSigningCredentials(JwtSigningAlgorithm.HMAC_SHA256, key)));

                Assert.IsTrue(TryValidateToken(token, new JwtValidationOptions(signingKey: key)));
            }

            [TestMethod, IntegrationTest]
            public void Validation_Fails_When_ExpirationTime_Is_In_The_Past_And_Validation_Requires_ExpirationTime_Validation()
            {
                var key = GetRandomKey();
                var token = GetTokenGenerator(DateTime.UtcNow.AddDays(-1)).Generate(null, new JwtGenerationOptions(
                    signingCredentials: new JwtSigningCredentials(JwtSigningAlgorithm.HMAC_SHA256, key)));

                Assert.IsFalse(TryValidateToken(token, new JwtValidationOptions(requireExpirationTime: true, signingKey: key)));
            }

            [TestMethod, IntegrationTest]
            public void Validation_Does_Not_Fail_When_ExpirationTime_Is_In_The_Past_And_Validation_Does_Not_Require_ExpirationTime_Validation()
            {
                var key = GetRandomKey();
                var token = GetTokenGenerator(DateTime.UtcNow.AddDays(-1)).Generate(null, new JwtGenerationOptions(
                    signingCredentials: new JwtSigningCredentials(JwtSigningAlgorithm.HMAC_SHA256, key)));

                Assert.IsTrue(TryValidateToken(token, new JwtValidationOptions(requireExpirationTime: false, signingKey: key)));
            }

            [TestMethod, IntegrationTest]
            public void Validation_Does_Not_Fail_When_ExpirationTime_Is_Within_Limits_And_Validation_Requires_ExpirationTime_Validation()
            {
                var key = GetRandomKey();
                var token = GetTokenGenerator(DateTime.UtcNow).Generate(null, new JwtGenerationOptions(
                    signingCredentials: new JwtSigningCredentials(JwtSigningAlgorithm.HMAC_SHA256, key)));

                Assert.IsTrue(TryValidateToken(token, new JwtValidationOptions(requireExpirationTime: true, signingKey: key)));
            }

            private static bool TryValidateToken(
                string securityToken,
                JwtValidationOptions options)
            {
                IPrincipal principal;
                return new JwtSecurityTokenHandler()
                    .TryValidateToken(securityToken, options, out principal);
            }

            private static JwtSecurityTokenGenerator GetTokenGenerator(                
                DateTime? now = null)
            {
                var tokenGenerator = new JwtSecurityTokenGenerator();

                tokenGenerator.SetDateTimeNowFactory(() => now ?? DateTime.UtcNow);

                return tokenGenerator;
            }

            private static string GetRandomKey(int size = 64)
            {
                var bytes = new byte[size];

                RandomNumberGenerator.Create().GetBytes(bytes);

                return Convert.ToBase64String(bytes);
            }
        }
    }
}
