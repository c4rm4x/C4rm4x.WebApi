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
    /// Returns the HTTP code 409 to indicate that request cannot be processed due a conflict
    /// </summary>
    public class ConflictResult : IHttpActionResult
    {
        /// <summary>
        /// Gets the reason why the request cannot be processed
        /// </summary>
        public string Reason { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reason">The conflict</param>
        public ConflictResult(string reason)
        {
            reason.NotNullOrEmpty(nameof(reason));

            Reason = reason;
        }

        /// <summary>
        ///  Creates an System.Net.Http.HttpResponseMessage asynchronously
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The task containing the HTTP response with the 409 code</returns>
        public Task<HttpResponseMessage> ExecuteAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            return HttpResponseMessageUtils
                .Create(HttpStatusCode.Conflict, Reason);
        }
    }
}
