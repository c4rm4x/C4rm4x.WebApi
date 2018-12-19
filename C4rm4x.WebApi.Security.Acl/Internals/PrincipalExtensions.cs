using C4rm4x.Tools.Utilities;
using System.Security.Claims;
using System.Security.Principal;

namespace C4rm4x.WebApi.Security.Acl
{
    internal static class PrincipalExtensions
    {
        public static string GetSharedSecret(
            this IPrincipal principal,
            string claimType)
        {
            principal.NotNull(nameof(principal));
            claimType.NotNullOrEmpty(nameof(claimType));
            principal.GetType().Is<ClaimsPrincipal>();

            return GetSharedSecret(principal as ClaimsPrincipal, claimType);
        }

        private static string GetSharedSecret(
            ClaimsPrincipal claimsPrincipal,
            string claimType)
        {
            return claimsPrincipal.FindFirst(claim => claim.Type == claimType)?.Value;
        }
    }
}
