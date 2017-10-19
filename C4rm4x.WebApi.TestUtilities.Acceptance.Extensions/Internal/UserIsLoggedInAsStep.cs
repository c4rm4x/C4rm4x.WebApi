#region Using

using C4rm4x.Tools.Security.Jwt;
using C4rm4x.Tools.Utilities;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;

#endregion

namespace C4rm4x.WebApi.TestUtilities.Acceptance.Internal
{
    /// <summary>
    /// Given-when-then step to generate a JWT with the given role/permissions
    /// </summary>
    internal class UserIsLoggedInAsStep
    {
        /// <summary>
        /// Gets the Http server responsible to handle requests
        /// </summary>
        public InMemoryHttpServer HttpServer { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpServer">The Http server</param>
        public UserIsLoggedInAsStep(InMemoryHttpServer httpServer)
        {
            httpServer.NotNull(nameof(httpServer));

            HttpServer = httpServer;
        }

        /// <summary>
        /// Retrieves a JWT token with the given role/permission and sets this
        /// at Http server to include it as a Authorization header for each request
        /// </summary>
        /// <param name="role">The role (if any)</param>
        /// <param name="permissions">The list of permissions (if any)</param>
        internal void TokenFor(
            string role = null,
            params KeyValuePair<string, object>[] permissions)
        {
            var claimsIdentity = GetClaimsIdentity(role, permissions);
            var token = new JwtSecurityTokenGenerator().Generate(claimsIdentity, GetOptions());

            HttpServer.SetSecurityToken(token);
        }

        private static ClaimsIdentity GetClaimsIdentity(
            string role = null,
            params KeyValuePair<string, object>[] permissions)
        {
            var claimsIdentity = new ClaimsIdentity(AuthenticationTypes.Password);

            AddRole(claimsIdentity, role);
            AddPermissions(claimsIdentity, permissions);

            return claimsIdentity;
        }

        private static void AddRole(
            ClaimsIdentity claimsIdentity,
            string role)
        {
            if (role.IsNullOrEmpty()) return;

            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        private static void AddPermissions(
            ClaimsIdentity claimsIdentity,
            params KeyValuePair<string, object>[] permissions)
        {
            foreach (var permission in permissions)
                claimsIdentity.AddClaim(new Claim(permission.Key, permission.Value.ToString()));
        }

        private static JwtGenerationOptions GetOptions()
        {
            return new JwtGenerationOptions(
                signingCredentials: new JwtSigningCredentials(
                    JwtSigningAlgorithm.HMAC_SHA256, GetSigningKey()));
        }

        private static string GetSigningKey()
        {
            return ConfigurationManager.AppSettings["JwtBasedSecurity.Secret"];
        }
    }
}
