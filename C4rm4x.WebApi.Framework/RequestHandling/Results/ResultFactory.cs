#region Using

using System;
using System.Collections.Generic;
using System.Web.Http;
using C4rm4x.WebApi.Framework.Validation;

#endregion

namespace C4rm4x.WebApi.Framework.RequestHandling.Results
{
    internal static class ResultFactory
    {
        public static IHttpActionResult Ok<TContent>(TContent content)
            where TContent : class
        {
            return new OkResult<TContent>(content);
        }

        public static IHttpActionResult Created<TContent>(TContent content)
            where TContent : class
        {
            return new CreatedResult<TContent>(content);
        }

        public static IHttpActionResult Accepted()
        {
            return new AcceptedResult();
        }

        public static IHttpActionResult NoContent()
        {
            return new NoContentResult();
        }

        public static IHttpActionResult Redirect(
            string url, 
            params KeyValuePair<string, object>[] query)
        {
            return new RedirectResult(url, query);
        }

        public static IHttpActionResult BadRequest(List<ValidationError> errors)
        {
            return new BadRequestResult(errors);
        }

        public static IHttpActionResult PaymentRequired(string reason)
        {
            return new PaymentRequiredResult(reason);
        }

        public static IHttpActionResult NotFound()
        {
            return new NotFoundResult();
        }

        public static IHttpActionResult Conflict(string reason)
        {
            return new ConflictResult(reason);
        }

        public static IHttpActionResult InternalServerError()
        {
            return new InternalServerErrorResult();
        }

        public static IHttpActionResult InternalServerError<TException>(
            TException exception)
            where TException : Exception
        {
            return new InternalServerErrorResult<TException>(exception);
        }
    }
}
