#region Using

using C4rm4x.Tools.Utilities;
using System.Collections.Generic;
using System.Web.Cors;

#endregion

namespace C4rm4x.WebApi.Security.Cors
{
    internal static class CorsEngineExtensions
    {
        public static bool EvaluateCorsPolicy(
            this CorsEngine corsEngine,
            CorsRequestContext context,
            CorsOptions options)
        {
            var result = corsEngine.EvaluatePolicy(context, options.GetCorsPolicy());

            return result.IsNotNull() && result.IsValid;
        }

        public static IDictionary<string, string> GetCorsResponseHeaders(
            this CorsEngine corsEngine,
            CorsRequestContext context,
            CorsOptions options)
        {
            return corsEngine
                .EvaluatePolicy(context, options.GetCorsPolicy())
                .ToResponseHeaders() ?? new Dictionary<string, string>();
        }
    }
}
