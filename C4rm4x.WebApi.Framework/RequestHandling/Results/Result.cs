#region Using

using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.RequestHandling.Results
{
    /// <summary>
    /// Returns the HTTP code given
    /// </summary>
    public class Result<TContent> : IHttpActionResult
    {
        /// <summary>
        /// Gets the status code to be returned
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Gets the content to be returned
        /// </summary>
        public TContent Content { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="statusCode">The status code</param>
        /// <param name="content">The content</param>
        public Result(HttpStatusCode statusCode, TContent content)
        {
            StatusCode = statusCode;
            Content = content;
        }

        /// <summary>
        ///  Creates an System.Net.Http.HttpResponseMessage asynchronously
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The task containing the HTTP response with the given code</returns>

        public Task<HttpResponseMessage> ExecuteAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            return HttpResponseMessageUtils.Create(StatusCode, Content);
        }
    }
}
