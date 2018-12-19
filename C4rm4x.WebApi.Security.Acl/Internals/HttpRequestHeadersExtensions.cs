using C4rm4x.Tools.Utilities;
using System.Linq;
using System.Net.Http.Headers;

namespace C4rm4x.WebApi.Security.Acl
{
    internal static class HttpRequestHeadersExtensions
    {
        public static string GetDigitalSignatureHeader(
            this HttpRequestHeaders headers,
            string name)
        {
            headers.NotNull(nameof(headers));
            name.NotNullOrEmpty(nameof(name));

            return headers.TryGetValues(name, out var values)
                ? values.FirstOrDefault()
                : null;
        }
    }
}
