#region Using

using C4rm4x.Tools.Utilities;
using System.Linq;
using System.Net.Http.Headers;

#endregion

namespace C4rm4x.WebApi.Security.WhiteList.Internals
{
    internal static class HttpRequestHeaderExtensions
    {
        public static string GetAuthorization(
            this HttpRequestHeaders headers)
        {
            const string AuthorizationHeader = "Authorization";

            return headers.GetHeaderOrDefault(AuthorizationHeader);
        }

        private static string GetHeaderOrDefault(
            this HttpRequestHeaders headers,
            string name)
        {
            headers.NotNull(nameof(headers));

            return headers.Contains(name)
                ? headers.GetValues(name).First()
                : null;
        }
    }
}
