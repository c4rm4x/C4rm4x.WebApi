#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Log;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Internal;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers
{
    /// <summary>
    /// Basic implementation of an ApiController responsible for checking 
    /// the health of your system
    /// </summary>
    public class ServiceStatusController : ApiController
    {
        private readonly ILog _logger;
        private readonly IEnumerable<IServiceStatusRetriever> 
            _serviceStatusRetrievers;

        private Func<IServiceStatusRequestHandler> _overallServiceStatusRequestHandlerFactory = 
            () => OverallServiceStatusHandler.GetInstance();

        private Func<IServiceStatusRequestHandler> _byComponentServiceStatusRequestHandlerFactory =
            () => ByComponentsServiceStatusHandler.GetInstance();


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="serviceStatusRetrievers">The list of all instances that implement interface IServiceStatusRetriever</param>
        public ServiceStatusController(
            ILog logger,
            IEnumerable<IServiceStatusRetriever> serviceStatusRetrievers)
        {
            logger.NotNull(nameof(logger));
            serviceStatusRetrievers.NotNullOrEmpty(nameof(serviceStatusRetrievers));

            _logger = logger;
            _serviceStatusRetrievers = serviceStatusRetrievers;
        }

        /// <summary>
        /// Checks the overall health of your system (or a collection of specific components within)
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>An instance of CheckHealthResponse with the health status of your system (or a collection of specific components within)</returns>
        [HttpGet]
        public IHttpActionResult CheckHealth(CheckHealthRequest request)
        {
            try
            {
                Validate(request);

                return Handle(request);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        private static void Validate(CheckHealthRequest request)
        {
            GetValidator()
                .ThrowIf(request);
        }

        private static IValidator<CheckHealthRequest> GetValidator()
        {
            return new CheckHealthRequestValidator();
        }

        private IHttpActionResult Handle(CheckHealthRequest request)
        {
            return GetHandler(request)
                .Handle(request);
        }

        private IServiceStatusRequestHandler GetHandler(CheckHealthRequest request)
        {
            var handler = (request.Components.IsNullOrEmpty())
                ? _overallServiceStatusRequestHandlerFactory()
                : _byComponentServiceStatusRequestHandlerFactory();

            handler.SetServiceStatusRetrievers(_serviceStatusRetrievers.AsEnumerable());

            return handler;
        }

        /// <summary>
        /// Handles any exception
        /// </summary>
        /// <param name="exception">The exception to be handled</param>
        /// <returns></returns>
        protected virtual IHttpActionResult HandleException(Exception exception)
        {
            if (exception is ValidationException)
                return BadRequest(exception as ValidationException);

            return HandleUnexpectedException(exception);
        }

        private IHttpActionResult HandleUnexpectedException(Exception exception)
        {
            _logger.Error("Unexpected exception", exception);

            return InternalServerError(
                new Exception("Unexpected server error. Please try again."));
        }

        private static IHttpActionResult BadRequest(ValidationException exception)
        {
            return new BadRequestResult(exception);
        }

        private new static IHttpActionResult InternalServerError(Exception exception)
        {
            return new InternalServerErrorResult<Exception>(exception);
        }

        /// <summary>
        /// Sets the overall service status request handler factory
        /// </summary>
        /// <param name="handlerFactory">The factory</param>
        /// <remarks>USE THIS ONLY FOR UNIT TESTING</remarks>
        internal void SetOverallServiceStatusHandlerFactory(
            Func<IServiceStatusRequestHandler> handlerFactory)
        {
            handlerFactory.NotNull(nameof(handlerFactory));

            _overallServiceStatusRequestHandlerFactory = handlerFactory;
        }

        /// <summary>
        /// Sets the by component service status request handler factory
        /// </summary>
        /// <param name="handlerFactory">The factory</param>
        /// <remarks>USE THIS ONLY FOR UNIT TESTING</remarks>
        internal void SetByComponentServiceStatusHandlerFactory(
            Func<IServiceStatusRequestHandler> handlerFactory)
        {
            handlerFactory.NotNull(nameof(handlerFactory));

            _byComponentServiceStatusRequestHandlerFactory = handlerFactory;
        }
    }
}
