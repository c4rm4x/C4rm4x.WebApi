#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Security.Cors
{
    /// <summary>
    /// Defines the policy for Cross-Origin requests based on the CORS specifications
    /// </summary>
    public class CorsOptions
    {
        /// <summary>
        /// The value for the Access-Control-Allow-Origin response header to allow all origins
        /// </summary>
        public const string AnyOrigin = "*";

        /// <summary>
        /// The value for the Access-Control-Allow-Origin response header to allow all methods
        /// </summary>
        public const string AnyMethod = "all";

        /// <summary>
        /// The value for the Access-Control-Allow-Origin response header to allow all headers
        /// </summary>
        public const string AnyHeader = "all";

        /// <summary>
        /// Gets a value indicating whether to allow all origins
        /// </summary>
        public bool AllowAnyOrigin { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to allow all methods
        /// </summary>
        public bool AllowAnyMethod { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to allow all headers
        /// </summary>
        public bool AllowAnyHeader { get; private set; }

        /// <summary>
        /// Gets the origins that are allowed to access the resource
        /// </summary>
        public IEnumerable<string> AllowedOrigins { get; private set; }

        /// <summary>
        /// Gets the methods that are supported by the resource
        /// </summary>
        public IEnumerable<string> AllowedMethods { get; private set; }

        /// <summary>
        /// Gets the headers that are supported by the resource
        /// </summary>
        public IEnumerable<string> AllowedHeaders { get; private set; }

        /// <summary>
        /// Gets the number of seconds the results of a preflight request can be cached
        /// </summary>
        public long? PreflightMaxAge { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the resource supports user credentials in the request
        /// </summary>
        public bool SupportsCredentials { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="allowedOriginsSeparatedByComma">Comma-separated list of allowed origins</param>
        /// <param name="allowedMethodsSeparatedByComma">Comma-separated list of allowed methods</param>
        /// <param name="allowedHeadersSeparatedByComma">Comma-separated list of allowed headers</param>
        /// <param name="supportsCredentials">Value indicating whether or not supports credentials in the request</param>
        /// <param name="preflightMaxAge">Number of seconds the result of a preflight request can be cached</param>
        public CorsOptions(
            string allowedOriginsSeparatedByComma = AnyOrigin,
            string allowedMethodsSeparatedByComma = "",
            string allowedHeadersSeparatedByComma = "",
            bool supportsCredentials = true,
            long? preflightMaxAge = null)
        {
            allowedOriginsSeparatedByComma.NotNullOrEmpty(nameof(allowedOriginsSeparatedByComma));            

            SupportsCredentials = supportsCredentials;
            PreflightMaxAge = preflightMaxAge;

            SetAllowedOrigins(allowedOriginsSeparatedByComma);
            SetAllowedMethods(allowedMethodsSeparatedByComma);
            SetAllowedHeaders(allowedHeadersSeparatedByComma);
        }

        private void SetAllowedOrigins(string allowedOriginsSeparatedByComma)
        {
            var allowedOrigins = Split(allowedOriginsSeparatedByComma);

            allowedOrigins.Must(x => x.Length > 0, "allowedOriginsSeparatedByComma must contain at least one origin");

            if (allowedOrigins.Length == 1 && allowedOrigins[0] == AnyOrigin)
                AllowAnyOrigin = true;
            else
                AllowedOrigins = allowedOrigins;
        }

        private void SetAllowedMethods(string allowedMethodsSeparatedByComma)
        {
            var allowedMethods = Split(allowedMethodsSeparatedByComma);

            if (allowedMethods.Length == 1 && allowedMethods[0] == AnyMethod)
                AllowAnyMethod = true;
            else
                AllowedMethods = allowedMethods;
        }

        private void SetAllowedHeaders(string allowedHeadersSeparatedByComma)
        {
            var allowedHeaders = Split(allowedHeadersSeparatedByComma);

            if (allowedHeaders.Length == 1 && allowedHeaders[0] == AnyHeader)
                AllowAnyHeader = true;
            else
                AllowedHeaders = allowedHeaders;
        }

        private static string[] Split(string valueSeparatedByComma)
        {
            if (valueSeparatedByComma.IsNullOrEmpty()) return new string[] { };

            return valueSeparatedByComma.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Returns a policy that allows all headers, all methods, any origin and supports credentials
        /// </summary>
        /// <param name="preflightMaxAge">Number of seconds the result of a preflight request can be cached</param>
        /// <returns></returns>
        public static CorsOptions AllowAll(long preflightMaxAge = 300)
        {
            return new CorsOptions(AnyOrigin, AnyMethod, AnyHeader, true, preflightMaxAge);
        }        
    }
}
