#region Using

using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Security
{
    /// <summary>
    /// Delegating handler responsible to check whether or not the security conditions
    /// match for the current HTTP request
    /// </summary>
    public abstract class SecurityMessageHandler : DelegatingHandler
    {
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous
        /// in which checks whether or not the request is allowed.
        /// In case the request is not allowed, a HTTP response with code 403 is returned instead
        /// </summary>
        /// <param name="request">The current HTTP request</param>
        /// <param name="cancellationToken">The cancellation token</param>
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (IsRequestAllowed(request))
                return Handle(request, cancellationToken);
            else
                return ForbiddenResponse();
        }

        private Task<HttpResponseMessage> Handle(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken)
                .ContinueWith(t => OnActionExecuted(t.Result, request, cancellationToken));
        }

        /// <summary>
        /// Action to be executed once the inner handler has finished its work 
        /// </summary>
        /// <param name="result">The HTTP response associated with the current HTTP request</param>
        /// <param name="request">The current HTTP request</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The response to return</returns>
        protected virtual HttpResponseMessage OnActionExecuted(
            HttpResponseMessage result,
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return result;
        }

        private Task<HttpResponseMessage> ForbiddenResponse()
        {
            return Task.FromResult(
                new HttpResponseMessage(ForbiddenErrorCode));
        }

        /// <summary>
        /// Gets the actual HttpStatusCode
        /// </summary>
        protected virtual HttpStatusCode ForbiddenErrorCode
        {
            get { return HttpStatusCode.Forbidden; }
        }

        /// <summary>
        /// Returns whether or not the current HTTP rquest is allowed to proceeed
        /// </summary>
        /// <param name="request">The current HTTP request</param>
        /// <returns>True if the current HTTP request is allowed; false, otherwise</returns>
        protected abstract bool IsRequestAllowed(
            HttpRequestMessage request);
    }
}
