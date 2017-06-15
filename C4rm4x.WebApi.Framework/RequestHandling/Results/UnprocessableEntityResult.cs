#region Using

using C4rm4x.Tools.Utilities;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.RequestHandling.Results
{
    /// <summary>
    /// Returns the HTTP code 422 to indicate that server is unable to process the contained instructions
    /// </summary>
    public class UnprocessableEntityResult : IHttpActionResult
    {
        /// <summary>
        /// Gets the reason why the request failed to be processed
        /// </summary>
        public string Reason { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reason">The reason</param>
        public UnprocessableEntityResult(string reason)
        {
            reason.NotNullOrEmpty(nameof(reason));

            Reason = reason;
        }

        /// <summary>
        ///  Creates an System.Net.Http.HttpResponseMessage asynchronously
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The task containing the HTTP response with the 422 code</returns>
        public Task<HttpResponseMessage> ExecuteAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            return HttpResponseMessageUtils.Create((HttpStatusCode)422, Reason);
        }
    }
}
