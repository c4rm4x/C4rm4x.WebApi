#region Using

using C4rm4x.Tools.Utilities;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.RequestHandling.Results
{
    /// <summary>
    /// Returns the HTTP code 200 to indicate that everything goes OK
    /// </summary>
    public class ContentResult : IHttpActionResult
    {
        /// <summary>
        /// Gets the content to be attached to the response as byte array
        /// </summary>
        public byte[] Content { get; private set; }

        /// <summary>
        /// Gets the mime type that describes how to treat the content
        /// </summary>
        public string MimeType { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">The content as byte array to be added with the response</param>
        /// <param name="mimeType">Mime type</param>
        public ContentResult(
            byte[] content,
            string mimeType)
        {
            content.NotNull(nameof(content));
            mimeType.NotNullOrEmpty(nameof(mimeType));

            Content = content;
            MimeType = mimeType;
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
            var response = HttpResponseMessageUtils.Create(
                HttpStatusCode.OK, 
                new ByteArrayContent(Content));

            response.Content.Headers.ContentType =
                new MediaTypeHeaderValue(MimeType);

            return response;
        }
    }
}
