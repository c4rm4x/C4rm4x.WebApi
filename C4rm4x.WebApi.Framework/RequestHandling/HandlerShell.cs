#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.ExceptionShielding;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Framework.Runtime;
using C4rm4x.WebApi.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// Gets the policy name for ExceptionPolicy
        /// </summary>
        string PolicyName { get; }

        /// <summary>
        /// Runs the code of the actual handler after all the common operations that are 
        /// performed for each request regardless of its type
        /// </summary>
        /// <typeparam name="TRequest">Type of the request</typeparam>
        /// <param name="request">The request to be handled</param>
        Task<IHttpActionResult> HandleAsync<TRequest>(TRequest request)
            where TRequest : ApiRequest;     
    }

    #endregion

    /// <summary>
    /// Implementation of IHandlerShell which performs request validator and 
    /// initialises the execution context before the actual handler gets executed
    /// </summary>
    public class HandlerShell : IHandlerShell
    {
        private readonly IValidatorFactory _validators;
        private readonly IPolicyNameFactory _policyNames;
        private readonly IExecutionContextInitialiser _executionContextInitialiser;
        private readonly IHandlerFactory _handlerFactory;

        /// <summary>
        /// Gets the policy name for ExceptionPolicy
        /// </summary>
        public string PolicyName
        {
            get { return _policyNames.Get(); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validators">Validator factory</param>
        /// <param name="policyNames">Policy name factory</param>
        /// <param name="executionContextInitialiser">Execution context initialiser</param>
        /// <param name="handlerFactory">The request handler factory</param>
        public HandlerShell(
            IValidatorFactory validators,
            IPolicyNameFactory policyNames,
            IExecutionContextInitialiser executionContextInitialiser,
            IHandlerFactory handlerFactory)
        {
            validators.NotNull(nameof(validators));
            policyNames.NotNull(nameof(policyNames));
            executionContextInitialiser.NotNull(nameof(executionContextInitialiser));
            handlerFactory.NotNull(nameof(handlerFactory));

            _validators = validators;
            _policyNames = policyNames;
            _executionContextInitialiser = executionContextInitialiser;
            _handlerFactory = handlerFactory;
        }

        /// <summary>
        /// Runs the code of the actual handler after all the common operations that are
        /// performed for each request regardless of its type
        /// </summary>
        /// <typeparam name="TRequest">Type of the request</typeparam>
        /// <param name="request">The request to be handled</param>        
        public async Task<IHttpActionResult> HandleAsync<TRequest>(TRequest request)
            where TRequest : ApiRequest
        {
            try
            {
                // Initialise context first so every piece of information required to 
                // proces the request is fetched beforehand
                await InitialiseContextAsync(request);

                // Validate the request (sintax and domain validation)
                // Wikipedia: Bad request when this cannot be fulfilled due to bad syntax
                // and / or would cause an invalid state
                var errors = await ValidateAsync(request);

                if (errors.Any())
                    return ResultFactory.BadRequest(errors);

                // Handles the request with all needed data already fetched
                // and ensuring it won't cause any invalid state
                return await GetHandler<TRequest>().HandleAsync(request);
            }
            catch (AggregateException e)
            {
                return HandleException(e.Flatten());
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        private Task InitialiseContextAsync<TRequest>(TRequest request)
            where TRequest : ApiRequest
        {
            return _executionContextInitialiser.PerRequestAsync(request);
        }

        private Task<List<ValidationError>> ValidateAsync<TRequest>(TRequest request)
            where TRequest : ApiRequest
        {
            return _validators
                .GetValidator(request.GetType())
                .ValidateAsync(request);
        }

        private IHandler<TRequest> GetHandler<TRequest>()
            where TRequest : ApiRequest
        {
            return _handlerFactory.GetHandler<TRequest>();
        }

        private IHttpActionResult HandleException(Exception exceptionToHandle)
        {
            Exception exceptionToThrow;

            if (ExceptionPolicy.HandleException(exceptionToHandle, PolicyName, out exceptionToThrow))
                return ResultFactory.InternalServerError<Exception>(
                    exceptionToThrow ?? exceptionToHandle);

            return ResultFactory.InternalServerError();
        }
    }
}
