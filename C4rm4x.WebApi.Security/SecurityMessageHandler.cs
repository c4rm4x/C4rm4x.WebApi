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
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (await IsRequestAllowedAsync(request))
                return await HandleAsync(request, cancellationToken);
            else
                return await ForbiddenResponseAsync();
        }

        /// <summary>
        /// Handles a valid request
        /// Default implementation sends an HTTP request to the inner handler
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation</param>
        /// <returns>Returns a task to produce the HTTP response</returns>
        /// <remarks>ALWAYS return base implementation to invoke inner handlers</remarks>
        protected virtual async Task<HttpResponseMessage> HandleAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<HttpResponseMessage> ForbiddenResponseAsync()
        {
            return await Task.FromResult(
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
        protected abstract Task<bool> IsRequestAllowedAsync(
            HttpRequestMessage request);
    }
}
