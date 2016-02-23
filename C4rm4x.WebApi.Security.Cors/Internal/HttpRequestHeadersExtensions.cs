#region Using

using System.Linq;
using System.Net.Http.Headers;
using System.Web.Cors;

#endregion

namespace C4rm4x.WebApi.Security.Cors
{
    internal static class HttpRequestHeadersExtensions
    {
        public static string GetOrigin(
            this HttpRequestHeaders headers)
        {
            return headers.GetHeaderOrDefault(CorsConstants.Origin);
        }

        public static string GetAccessControlRequestMethod(
            this HttpRequestHeaders headers)
        {
            return headers.GetHeaderOrDefault(CorsConstants.AccessControlRequestMethod);
        }

        public static string GetAccessControlRequestHeaders(
            this HttpRequestHeaders headers)
        {
            return headers.GetHeaderOrDefault(CorsConstants.AccessControlRequestHeaders);
        }

        private static string GetHeaderOrDefault(
            this HttpRequestHeaders headers,
            string name)
        {
            return headers.Contains(name)
                ? headers.GetValues(name).First()
                : null;
        }
    }
}
