#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Validation;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.RequestHandling.Results
{
    /// <summary>
    /// Returns the HTTP code 400 to indicate the request is invalid
    /// </summary>
    public class BadRequestResult : IHttpActionResult
    {
        /// <summary>
        /// Gets the ValidationException instance that makes the request fail
        /// </summary>
        public ValidationException Exception { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exception">The validation exception</param>
        public BadRequestResult(ValidationException exception)
        {
            exception.NotNull(nameof(exception));

            Exception = exception;
        }

        /// <summary>
        ///  Creates an System.Net.Http.HttpResponseMessage asynchronously
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The task containing the HTTP response with the 403 code</returns>        
        public Task<HttpResponseMessage> ExecuteAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            return HttpResponseMessageUtils.Create(
                HttpStatusCode.BadRequest,
                Exception.ValidationErrors.Select(
                    e => new 
                    {
                        PropertyName = e.PropertyName,
                        ErrorDescription = e.ErrorDescription,
                    }
                ));
        }
    }
}
