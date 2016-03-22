#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.ExceptionShielding;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Framework.Runtime;
using C4rm4x.WebApi.Framework.Validation;
using System;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.RequestHandling
{
    #region Interface

    /// <summary>
    /// Service responsible to shell every request and performs common operations
    /// </summary>
    public interface IHandlerShell
    {
        /// <summary>
        /// Sets the policy name for this request
        /// </summary>
        string PolicyName { set; }

        /// <summary>
        /// Runs the code of the actual handler after all the common operations that are 
        /// performed for each request regardless of its type
        /// </summary>
        /// <typeparam name="TRequest">Type of the request</typeparam>
        /// <typeparam name="TResponse">Type of the response</typeparam>
        /// <param name="request">The request to be handled</param>
        /// <param name="processor">The processor responsible of actually handling the request</param>
        IHttpActionResult Process<TRequest, TResponse>(
            TRequest request,
            Func<TRequest, TResponse> processor)
            where TRequest : ApiRequest
            where TResponse : ApiResponse;

        /// <summary>
        /// Runs the code of the actual handler after all the common operations that are
        /// performed for each request regardless of its type
        /// </summary>
        /// <typeparam name="TRequest">Type of the request</typeparam>
        /// <param name="request">The request to be handled</param>
        /// <param name="processor">The processor responsible of actually handling the request</param>
        IHttpActionResult Process<TRequest>(
            TRequest request,
            Func<TRequest, IHttpActionResult> processor)
            where TRequest : ApiRequest;        
    }

    #endregion

    /// <summary>
    /// Implementation of IHandlerShell which performs request validator and 
    /// initialises the execution context before the actual handler gets executed
    /// </summary>
    [DomainService(typeof(IHandlerShell))]
    public class HandlerShell : IHandlerShell
    {
        private readonly IValidatorFactory _validators;
        private readonly IExecutionContextInitialiser _executionContextInitialiser;

        /// <summary>
        /// Sets the policy name for ExceptionPolicy
        /// </summary>
        public string PolicyName { private get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validators">Validator factory</param>
        /// <param name="executionContextInitialiser">Execution context initialiser</param>
        public HandlerShell(
            IValidatorFactory validators,
            IExecutionContextInitialiser executionContextInitialiser)
        {
            validators.NotNull(nameof(validators));
            executionContextInitialiser.NotNull(nameof(executionContextInitialiser));

            _validators = validators;
            _executionContextInitialiser = executionContextInitialiser;
        }

        /// <summary>
        /// Runs the code of the actual handler after all the common operations that are 
        /// performed for each request regardless of its type
        /// </summary>
        /// <typeparam name="TRequest">Type of the request</typeparam>
        /// <typeparam name="TResponse">Type of the response</typeparam>
        /// <param name="request">The request to be handled</param>
        /// <param name="processor">The processor responsible of actually handling the request</param>
        public IHttpActionResult Process<TRequest, TResponse>(
            TRequest request,
            Func<TRequest, TResponse> processor)
            where TRequest : ApiRequest
            where TResponse : ApiResponse
        {
            return Process(request, r => Ok(processor(r)));
        }

        /// <summary>
        /// Runs the code of the actual handler after all the common operations that are
        /// performed for each request regardless of its type
        /// </summary>
        /// <typeparam name="TRequest">Type of the request</typeparam>
        /// <param name="request">The request to be handled</param>
        /// <param name="processor">The processor responsible of actually handling the request</param>
        public IHttpActionResult Process<TRequest>(
            TRequest request,
            Func<TRequest, IHttpActionResult> processor)
            where TRequest : ApiRequest
        {
            processor.NotNull(nameof(processor));

            try
            {
                Validate(request);

                InitialiseContext(request);

                return processor(request);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        private void Validate<TRequest>(TRequest request)
            where TRequest : ApiRequest
        {
            _validators
                .GetValidator(request.GetType())
                .ThrowIf(request);
        }

        private void InitialiseContext<TRequest>(TRequest request)
            where TRequest : ApiRequest
        {
            _executionContextInitialiser.PerRequest(request);
        }

        private IHttpActionResult HandleException(Exception exceptionToHandle)
        {
            Exception exceptionToThrow;

            if (ExceptionPolicy.HandleException(exceptionToHandle, PolicyName, out exceptionToThrow))
                return Error(exceptionToThrow ?? exceptionToHandle);

            return GenericError();
        }

        private static IHttpActionResult Error(Exception exception)
        {
            return (exception is ValidationException)
                ? BadRequest(exception as ValidationException)
                : InternalServerError(exception);
        }

        /// <summary>
        /// Swallows exception and returns generic error with no information provided
        /// </summary>
        /// <remarks>
        /// Ideally this should never happen but depends on exception handling configuration...
        /// </remarks>
        private static IHttpActionResult GenericError()
        {
            return InternalServerError();
        }

        private static IHttpActionResult Ok<TResponse>(TResponse response)
            where TResponse : ApiResponse
        {
            return new OkResult<TResponse>(response);
        }

        private static IHttpActionResult BadRequest(ValidationException exception)
        {
            return new BadRequestResult(exception);
        }

        private static IHttpActionResult InternalServerError(Exception exception)
        {
            return new InternalServerErrorResult<Exception>(exception);
        }

        private static IHttpActionResult InternalServerError()
        {
            return new InternalServerErrorResult();
        }
    }
}
