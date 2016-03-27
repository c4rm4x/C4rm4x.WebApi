#region Using

using C4rm4x.Tools.Utilities;
using System.Net.Http;
using System.Security.Principal;

#endregion

namespace C4rm4x.WebApi.Security.Jwt
{
    internal static class HttpRequestMessageExtensions
    {
        private const string PrincipalKey = "MS_UserPrincipal";

        public static void SetUserPrincipal(
            this HttpRequestMessage requestMessage,
            IPrincipal principal)
        {
            requestMessage.NotNull(nameof(requestMessage));

            if (principal.IsNull()) return;

            requestMessage.Properties[PrincipalKey] = principal;
        }

        public static IPrincipal GetUserPrincipal(
            this HttpRequestMessage requestMessage)
        {
            requestMessage.NotNull(nameof(requestMessage));

            object value;
            if (requestMessage.Properties.TryGetValue(PrincipalKey, out value))
                return value as IPrincipal;

            return null;
        }
    }
}
