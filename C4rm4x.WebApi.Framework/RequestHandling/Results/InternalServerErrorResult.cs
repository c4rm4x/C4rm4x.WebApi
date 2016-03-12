#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.RequestHandling.Results
{
    /// <summary>
    /// Returns the HTTP code 500 to indicate an error has occurred in the server
    /// </summary>
    public class InternalServerErrorResult : IHttpActionResult
    {
        /// <summary>
        ///  Creates an System.Net.Http.HttpResponseMessage asynchronously
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The task containing the HTTP response with the 500 code</returns>
        public Task<HttpResponseMessage> ExecuteAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        /// <summary>
        /// Returns the HTTP response
        /// </summary>
        /// <returns>Returns the HTTP code 500</returns>
        protected virtual HttpResponseMessage Execute()
        {
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    /// Returns the HTTP code 500 to indicate an error has occurred in the server
    /// including information about the error itself
    /// </summary>
    public class InternalServerErrorResult<TException> :
        InternalServerErrorResult
        where TException : Exception
    {
        /// <summary>
        /// Gets the exception that originated the HTTP code 500
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exception">The exception that originated the error</param>
        public InternalServerErrorResult(Exception exception)
        {
            exception.NotNull(nameof(exception));

            Exception = exception;
        }

        /// <summary>
        /// Returns the HTTP response
        /// </summary>
        /// <returns>Returns the HTTP code 500 including informaion about the error itself</returns>
        protected override HttpResponseMessage Execute()
        {
            return HttpResponseMessageUtils.Create(
                HttpStatusCode.InternalServerError,
                GetErrorObject());
        }
        
        private static object GetErrorObject()
        {
            var exceptionAsApiException = Exception as ApiException;
            
            return (exceptionAsApiException.IsNotNull())
                ? new 
                {
                    Code = exceptionAsApiException.Code,
                    Message = Exception.Message,
                }
                : new HttpError(Exception.Message);
        }
    }
}
