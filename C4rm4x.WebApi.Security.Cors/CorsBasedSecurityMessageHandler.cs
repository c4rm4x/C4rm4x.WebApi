#region Using

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
        /// Gets the CORS policy to be applied
        /// </summary>
        public CorsPolicy Policy { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="policy">The CORS policy to be applied</param>
        public CorsBasedSecurityMessageHandler(
            CorsPolicy policy = null)
        {
            policy.NotNull(nameof(policy));

            Policy = policy;
        }

        /// <summary>
        /// Returns whether or not the current HTTP request is allowed to proceeed
        /// </summary>
        /// <param name="request">The current HTTP request</param>
        /// <returns>True if the current HTTP request is allowed; false, otherwise</returns>
        protected override bool IsRequestAllowed(HttpRequestMessage request)
        {
            var corsRequestContext = GetCorsRequestContext(request);

            if (corsRequestContext.IsNull()) // No CORS request -> Valid!
                return true;

            return _corsEngineFactory()
                .EvaluateCorsPolicy(corsRequestContext, Policy);
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
        protected override Task<HttpResponseMessage> Handle(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var corsRequestContext = GetCorsRequestContext(request);

            if (corsRequestContext.IsNull()) // No CORS request -> Send to inner handler
                return base.Handle(request, cancellationToken);

            return HandleCorsRequest(request, cancellationToken, corsRequestContext);
        }

        private Task<HttpResponseMessage> HandleCorsRequest(
            HttpRequestMessage request,
            CancellationToken cancellationToken,
            CorsRequestContext corsRequestContext)
        {
            if (corsRequestContext.IsPreflight)
                return HandleCorsPreflightRequest(corsRequestContext);

            return base.Handle(request, cancellationToken)
                .ContinueWith(responseTask =>
                {
                    var response = responseTask.Result;

                    WriteCorsHeaders(response, corsRequestContext);

                    return response;
                });
        }

        private Task<HttpResponseMessage> HandleCorsPreflightRequest(
            CorsRequestContext corsRequestContext)
        {
            // Is it a prefligh request but it does not have Access-Control-Request-Headers? -> Bad request
            if (corsRequestContext.AccessControlRequestMethod.IsNullOrEmpty())
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest));

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            WriteCorsHeaders(response, corsRequestContext);

            return Task.FromResult(response);
        }

        private void WriteCorsHeaders(
            HttpResponseMessage response,
            CorsRequestContext corsRequestContext)
        {
            var headers = _corsEngineFactory()
                .GetCorsResponseHeaders(corsRequestContext, Policy);

            foreach (var header in headers)
                response.Headers.Add(header.Key, header.Value);
        }

        /// <summary>
        /// Sets the CORS engine factory
        /// </summary>
        /// <param name="corsEngineFactory">The factory</param>
        /// <remarks>USE THIS ONLY FOR UNIT TESTING</remarks>
        public void SetCorsEngineFactory(Func<CorsEngine> corsEngineFactory)
        {
            corsEngineFactory.NotNull(nameof(corsEngineFactory));

            _corsEngineFactory = corsEngineFactory;
        }
    }
}
