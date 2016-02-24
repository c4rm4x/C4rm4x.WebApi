#region Using

using C4rm4x.Tools.Utilities;
using System.Collections.Generic;
using System.Web.Cors;

#endregion

namespace C4rm4x.WebApi.Security.Cors
{
    internal static class CorsOptionsExtensions
    {
        public static CorsPolicy GetCorsPolicy(
            this CorsOptions options)
        {
            options.NotNull(nameof(options));

            var corsPolicy = new CorsPolicy
            {
                AllowAnyHeader = options.AllowAnyHeader,
                AllowAnyMethod = options.AllowAnyMethod,
                AllowAnyOrigin = options.AllowAnyOrigin,
                SupportsCredentials = options.SupportsCredentials,
            };

            if (options.PreflightMaxAge.HasValue)
                corsPolicy.PreflightMaxAge = options.PreflightMaxAge;

            options.SetHeadersIfRequired(corsPolicy);
            options.SetMethodsIfRequired(corsPolicy);
            options.SetOriginsIfRequired(corsPolicy);

            return corsPolicy;
        }

        private static void SetHeadersIfRequired(
            this CorsOptions options,
            CorsPolicy corsPolicy)
        {
            if (options.AllowAnyHeader) return;

            SetList(options.AllowedHeaders, corsPolicy.Headers);
        }

        private static void SetMethodsIfRequired(
            this CorsOptions options,
            CorsPolicy corsPolicy)
        {
            if (options.AllowAnyMethod) return;

            SetList(options.AllowedMethods, corsPolicy.Methods);
        }

        private static void SetOriginsIfRequired(
            this CorsOptions options,
            CorsPolicy corsPolicy)
        {
            if (options.AllowAnyOrigin) return;

            SetList(options.AllowedOrigins, corsPolicy.Origins);
        }

        private static void SetList(
            IEnumerable<string> sources,
            IList<string> destinations)
        {
            foreach (var source in sources)
                destinations.Add(source);
        }
    }
}
