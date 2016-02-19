#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.IdentityModel.Tokens;
using System.Security.Principal;

#endregion

namespace C4rm4x.WebApi.Security.Jwt
{
    internal static class JwtSecurityTokenHandlerExtensions
    {
        public static bool TryValidateToken(
            this JwtSecurityTokenHandler handler,
            string securityToken,
            JwtValidationOptions options,
            out IPrincipal principal)
        {
            try
            {
                principal = handler
                    .RetrievePrincipal(securityToken, GetValidationParameters(options));
            }
            catch (SecurityTokenValidationException)
            {
                principal = null;
            }

            return principal.IsNotNull();
        }

        private static TokenValidationParameters GetValidationParameters(
            JwtValidationOptions options)
        {
            var parameters = new TokenValidationParameters
            {
                RequireExpirationTime = options.RequireExpirationTime,
                RequireSignedTokens = !options.SigningKey.IsNullOrEmpty(),
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = options.RequireExpirationTime,
            };

            if (!options.SigningKey.IsNullOrEmpty())
                parameters.IssuerSigningKey = new InMemorySymmetricSecurityKey(
                    Convert.FromBase64String(options.SigningKey));

            return parameters;
        }

        private static IPrincipal RetrievePrincipal(
            this JwtSecurityTokenHandler tokenHandler,
            string securityToken,
            TokenValidationParameters validationParameters)
        {
            SecurityToken validatedToken;
            return tokenHandler
                .ValidateToken(securityToken, validationParameters, out validatedToken);
        }
    }
}
