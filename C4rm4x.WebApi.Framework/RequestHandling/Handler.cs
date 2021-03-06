﻿#region Using

using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Framework.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.RequestHandling
{
    #region Interface

    /// <summary>
    /// Handles an specific type of request
    /// </summary>
    /// <typeparam name="TRequest">Type of request</typeparam>
    public interface IHandler<TRequest>
        where TRequest : ApiRequest
    {
        /// <summary>
        /// Handles a request of the specified type
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>Returns an instance of IHttpActionResult</returns>
        Task<IHttpActionResult> HandleAsync(TRequest request);
    }

    #endregion

    /// <summary>
    /// Base class that implements IHandler for an specific type of request
    /// </summary>
    /// <typeparam name="TRequest">Type of request</typeparam>
    public abstract class Handler<TRequest> : IHandler<TRequest>
        where TRequest : ApiRequest
    {
        /// <summary>
        /// Handles a request of the specified type
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>Returns an instance of IHttpActionResult</returns>
        public abstract Task<IHttpActionResult> HandleAsync(TRequest request);
        
        /// <summary>
        /// Returns an instance of IHttpActionResult with Code 200
        /// </summary>
        /// <typeparam name="TContent">Type of the content</typeparam>
        /// <param name="content">The content</param>
        /// <returns>An IHttpActionResult with status code 200</returns>
        protected IHttpActionResult Ok<TContent>(TContent content)
            where TContent : class
        {
            return ResultFactory.Ok(content);
        }

        /// <summary>
        /// Returns an instance of IHttpActionResult with Code 200
        /// </summary>
        /// <param name="content">Content</param>
        /// <param name="mime">Mime-type</param>
        /// <returns>An IHttpActionResult with status code 200</returns>
        protected IHttpActionResult Content(byte[] content, string mime)
        {
            return ResultFactory.Content(content, mime);
        }

        /// <summary>
        /// Returns an instance of IHttpActionResult with Code 201
        /// </summary>
        /// <typeparam name="TContent">Type of the content</typeparam>
        /// <param name="content">The content</param>
        /// <returns>An IHttpActionResult with status code 201</returns>
        protected IHttpActionResult Created<TContent>(
            TContent content)
            where TContent : class
        {
            return ResultFactory.Created(content);
        }

        /// <summary>
        /// Returns an instance of IHttpActionResult with Code 202
        /// </summary>
        /// <returns>An IHttpActionResult with status code 202</returns>
        protected IHttpActionResult Accepted()
        {
            return ResultFactory.Accepted();
        }

        /// <summary>
        /// Returns an instance of IHttpActionResult with Code 204
        /// </summary>
        /// <returns>An IHttpActionResult with status code 204</returns>
        protected IHttpActionResult NoContent()
        {
            return ResultFactory.NoContent();
        }

        /// <summary>
        /// Returns an instance of IHttpActionResult with Code 302
        /// </summary>
        /// <param name="url">The url to redirect to</param>
        /// <param name="query">The optional query string</param>
        /// <returns>An IHttpActionResult with status code 302</returns>
        protected IHttpActionResult Redirect(
            string url,
            params KeyValuePair<string, object>[] query)
        {
            return ResultFactory.Redirect(url, query);
        }

        /// <summary>
        /// Returns an instance of IHttpActionResult with Code 402
        /// </summary>
        /// <param name="reason">How to prevent this error</param>
        /// <returns>An IHttpActionResult with status code 402</returns>
        protected IHttpActionResult PaymentRequired(string reason)
        {
            return ResultFactory.PaymentRequired(reason);
        }

        /// <summary>
        /// Returns an instance of IHttpActionResult with Code 404
        /// </summary>
        /// <returns>An IHttpActionResult with status code 404</returns>
        protected IHttpActionResult NotFound()
        {
            return ResultFactory.NotFound();
        }

        /// <summary>
        /// Returns an instance of IHttpActionResult with Code 409
        /// </summary>
        /// <param name="reason">The conflict description</param>
        /// <returns>An IHttpActionResult with status code 409</returns>
        protected IHttpActionResult Conflict(string reason)
        {
            return ResultFactory.Conflict(reason);
        }

        /// <summary>
        /// Returns an instance of IHttpActionResult with Code 422
        /// </summary>
        /// <param name="reason">The conflict description</param>
        /// <returns>An IHttpActionResult with status code 422</returns>
        protected IHttpActionResult UnprocessableEntity(string reason)
        {
            return ResultFactory.UnprocessableEntity(reason);
        }

        /// <summary>
        /// Returns an instance of IHttpActionResult with given StatusCode
        /// </summary>
        /// <typeparam name="TContent">Type of the content</typeparam>
        /// <param name="statusCode">The status code</param>
        /// <param name="content">The content</param>
        /// <returns>An IHttpActionResult with given status code</returns>
        /// <remarks>USE WITH CAUTION</remarks>
        protected IHttpActionResult Result<TContent>(
            HttpStatusCode statusCode, TContent content)
        {
            return ResultFactory.Result<TContent>(statusCode, content);
        }
    }
}
