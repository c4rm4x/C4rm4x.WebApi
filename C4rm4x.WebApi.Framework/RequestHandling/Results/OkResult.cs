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
    /// Returns the HTTP code 200 to indicate that everything goes OK
    /// </summary>
    public class OkResult<TContent> : IHttpActionResult
        where TContent : class
    {
        /// <summary>
        /// Gets the content to be attached to the response
        /// </summary>
        public TContent Content { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">The content to be added as part of the HTTP response</param>
        public OkResult(TContent content)
        {
            content.NotNull(nameof(content));

            Content = content;
        }

        /// <summary>
        ///  Creates an System.Net.Http.HttpResponseMessage asynchronously
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The task containing the HTTP response with the 200 code</returns>
        public Task<HttpResponseMessage> ExecuteAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            return HttpResponseMessageUtils.Create<TContent>(
                HttpStatusCode.OK, Content);
        }
    }
}
