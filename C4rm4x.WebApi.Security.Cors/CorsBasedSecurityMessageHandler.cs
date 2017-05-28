﻿#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;

#endregion

namespace C4rm4x.WebApi.Security.Cors
{
    /// <summary>
    /// Delegating handler responsible to check whether or not the CORS policy is fulfilled
    /// for the current HTTP request
    /// </summary>
    public class CorsBasedSecurityMessageHandler : SecurityMessageHandler
    {
        private Func<CorsEngine> _corsEngineFactory =
            () => new CorsEngine();

        /// <summary>
        /// Gets the CORS options to be applied
        /// </summary>
        public CorsOptions Options { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">The CORS options to be applied</param>
        public CorsBasedSecurityMessageHandler(
            CorsOptions options = null)
        {
            options.NotNull(nameof(options));

            Options = options;
        }

        /// <summary>
        /// Returns whether or not the current HTTP request is allowed to proceeed
        /// </summary>
        /// <param name="request">The current HTTP request</param>
        /// <returns>True if the current HTTP request is allowed; false, otherwise</returns>
        protected override async Task<bool> IsRequestAllowedAsync(HttpRequestMessage request)
        {
            var corsRequestContext = GetCorsRequestContext(request);

            if (corsRequestContext.IsNull()) // No CORS request -> Valid!
                return true;

            return await Task.FromResult(_corsEngineFactory()
                .EvaluateCorsPolicy(corsRequestContext, Options));
        }

        private CorsRequestContext GetCorsRequestContext(
            HttpRequestMessage request)
        {
            var origin = request.Headers.GetOrigin();

            if (origin.IsNullOrEmpty()) return null;

            var context = new CorsRequestContext
            {
                RequestUri = request.RequestUri,
                HttpMethod = request.Method.ToString(),
                Host = request.Headers.Host,
                Origin = origin,
                AccessControlRequestMethod = request.Headers.GetAccessControlRequestMethod(),                
            };

            context.SetAccessControlRequestHeaders(
                request.Headers.GetAccessControlRequestHeaders());

            return context;
        }

        /// <summary>
        /// Handles a valid request
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation</param>
        /// <returns>Returns a task to produce the HTTP response</returns>
        protected override async Task<HttpResponseMessage> HandleAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var corsRequestContext = GetCorsRequestContext(request);

            if (corsRequestContext.IsNull()) // No CORS request -> Send to inner handler
                return await base.HandleAsync(request, cancellationToken);

            return await HandleCorsRequestAsync(request, cancellationToken, corsRequestContext);
        }

        private async Task<HttpResponseMessage> HandleCorsRequestAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken,
            CorsRequestContext corsRequestContext)
        {
            if (corsRequestContext.IsPreflight)
                return await HandleCorsPreflightRequestAsync(corsRequestContext);

            return await base.HandleAsync(request, cancellationToken)
                .ContinueWith(responseTask =>
                {
                    var response = responseTask.Result;

                    WriteCorsHeaders(response, corsRequestContext);

                    return response;
                });
        }

        private async Task<HttpResponseMessage> HandleCorsPreflightRequestAsync(
            CorsRequestContext corsRequestContext)
        {           
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            WriteCorsHeaders(response, corsRequestContext);

            return await Task.FromResult(response);
        }

        private void WriteCorsHeaders(
            HttpResponseMessage response,
            CorsRequestContext corsRequestContext)
        {
            var headers = _corsEngineFactory()
                .GetCorsResponseHeaders(corsRequestContext, Options);

            foreach (var header in headers)
                response.Headers.Add(header.Key, header.Value);
        }

        /// <summary>
        /// Sets the CORS engine factory
        /// </summary>
        /// <param name="corsEngineFactory">The factory</param>
        /// <remarks>USE THIS ONLY FOR UNIT TESTING</remarks>
        internal void SetCorsEngineFactory(Func<CorsEngine> corsEngineFactory)
        {
            corsEngineFactory.NotNull(nameof(corsEngineFactory));

            _corsEngineFactory = corsEngineFactory;
        }
    }
}
