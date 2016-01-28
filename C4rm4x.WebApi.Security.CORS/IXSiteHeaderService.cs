#region Using

using System.Net.Http;

#endregion

namespace C4rm4x.WebApi.Security.CORS
{
    /// <summary>
    /// Service responsible to manage Cross-Site security logic related 
    /// </summary>
    public interface IXSiteHeaderService
    {
        /// <summary>
        /// Returns whether or not the referrer is allowed
        /// </summary>
        /// <param name="request">The instance of the current HTTP request</param>
        /// <returns>True when the referrer is allowed. False, otherwise</returns>
        bool IsReferrerAllowed(HttpRequestMessage request);

        /// <summary>
        /// Adds Access-Control-Allow-Origin header to the HTTP response
        /// </summary>
        /// <param name="request">The current HTTP request</param>
        /// <param name="response">The HTTP response associated</param>
        void AddResponseHeader(
            HttpRequestMessage request, 
            HttpResponseMessage response);
    }
}
