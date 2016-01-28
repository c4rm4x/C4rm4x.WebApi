#region Using

using C4rm4x.Tools.Utilities;
using System.Net.Http;
using System.Threading;

#endregion

namespace C4rm4x.WebApi.Security.CORS
{
    /// <summary>
    /// Implementaion of the service SecurityMessageHandler responsible to reject all
    /// the HTTP request where the referrer is not included in WhiteListUrl storage
    /// </summary>
    public class XSiteHeaderMessageHandler : SecurityMessageHandler
    {
        private readonly IXSiteHeaderService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">The instance of the service IVeCrossSiteHeaderService</param>
        public XSiteHeaderMessageHandler(
            IXSiteHeaderService service)
        {
            service.NotNull(nameof(service));

            _service = service;
        }

        /// <summary>
        /// Returns whether or not the current HTTP request referrer is allowed
        /// </summary>
        /// <param name="request">The current HTTP request</param>
        /// <returns>True when the referrer is allowed to proceed. False, otherwise</returns>
        protected override bool IsRequestAllowed(HttpRequestMessage request)
        {
            return _service.IsReferrerAllowed(request);
        }

        /// <summary>
        /// Executes the action to be performed once the request has been handled
        /// </summary>
        /// <param name="result">The HTTP response associated to the current HTTP request</param>
        /// <param name="request">The current HTTP request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Adds header to the HTTP response</returns>
        protected override HttpResponseMessage OnActionExecuted(
            HttpResponseMessage result,
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            _service.AddResponseHeader(request, result);

            return base.OnActionExecuted(result, request, cancellationToken);
        }
    }
}
