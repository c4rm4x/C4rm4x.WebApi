#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.RequestHandling.Results
{
    /// <summary>
    /// Returns the HTTP code 302 to indicate that response should be re-directed
    /// </summary>
    public class RedirectResult : IHttpActionResult
    {
        /// <summary>
        /// Gets the location to which the request is redirected
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Gets the collection of query string parameters as KeyValuePair
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> QueryStringParameters { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url">The location to which the request is redirected</param>
        /// <param name="parameters">Collection of query string parameters</param>
        public RedirectResult(string url,
            params KeyValuePair<string, object>[] parameters)
        {
            url.NotNullOrEmpty(nameof(url));

            Url = url;
            QueryStringParameters = parameters ?? new KeyValuePair<string, object>[] { };
        }

        /// <summary>
        ///  Creates an System.Net.Http.HttpResponseMessage asynchronously
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The task containing the HTTP response with the 302 code</returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Redirect);

            response.Headers.Location = new Uri(GetDestinationUrl());

            return response;
        }

        private string GetDestinationUrl()
        {
            var sb = new StringBuilder(Url);

            if (QueryStringParameters.Any())
            {
                var prefix = Url.Contains("?") ? "&" : "?";

                sb.AppendFormat("{0}{1}", prefix, GetQueryString());
            }

            return sb.ToString();
        }

        private string GetQueryString()
        {
            return string.Join("&", QueryStringParameters.Select(q =>
                "{0}={1}".AsFormat(q.Key, q.Value)));
        }
    }
}
