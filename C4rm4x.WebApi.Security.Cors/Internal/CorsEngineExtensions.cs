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
            CorsPolicy policy)
        {
            var result = corsEngine.EvaluatePolicy(context, policy);

            return result.IsNotNull() && result.IsValid;
        }

        public static IDictionary<string, string> GetCorsResponseHeaders(
            this CorsEngine corsEngine,
            CorsRequestContext context,
            CorsPolicy policy)
        {
            return corsEngine
                .EvaluatePolicy(context, policy)
                .ToResponseHeaders() ?? new Dictionary<string, string>();
        }
    }
}
