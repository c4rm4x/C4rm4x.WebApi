#region Using

using C4rm4x.Tools.Utilities;
using System.Net.Http.Headers;

#endregion

namespace C4rm4x.WebApi.Security.Jwt
{
    internal static class AuthenticationHeaderValueExtensions
    {
        private const string Authorization_Scheme_Bearer = "Bearer";

        public static bool IsEmptyOrBearer(
            this AuthenticationHeaderValue authorizationHeaderValue)
        {
            authorizationHeaderValue.NotNull("authorizationHeaverValue");

            return authorizationHeaderValue.Parameter.IsNullOrEmpty() ||
                authorizationHeaderValue.Scheme.Equals(Authorization_Scheme_Bearer);
        }

        public static string GetBearerToken(
            this AuthenticationHeaderValue authorizationHeaderValue)
        {
            authorizationHeaderValue.NotNull("authorizationHeaverValue");

            if (!authorizationHeaderValue.IsEmptyOrBearer())
                return string.Empty;

            return authorizationHeaderValue.Parameter.IsNullOrEmpty()
                ? authorizationHeaderValue.Scheme
                : authorizationHeaderValue.Parameter;
        }
    }
}
