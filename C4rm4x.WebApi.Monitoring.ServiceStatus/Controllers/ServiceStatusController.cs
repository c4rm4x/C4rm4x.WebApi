#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Log;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers
{
    /// <summary>
    /// Basic implementation of an ApiController responsible for checking the health of your system
    /// </summary>
    public class ServiceStatusController : ApiController
    {
        private readonly ILog _logger;
        private readonly IEnumerable<IServiceStatusRetriever> 
            _serviceStatusRetrievers;

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
                return Handle(request);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        private IHttpActionResult Handle(CheckHealthRequest request)
        {
            return GetHandler(request)
                .Handle(request);
        }

        private IServiceStatusRequestHandler GetHandler(CheckHealthRequest request)
        {
            // Fancy factory !! 
            // The whole thing needs to be re-designed because it is a piece of sh*t !
            if (request is CheckOverallHealthRequest)
                return OverallServiceStatusHandler.GetInstance(_serviceStatusRetrievers);
            else if (request is CheckComponentsHealthRequest)
                return ByComponentsServiceStatusHandler.GetInstance(_serviceStatusRetrievers);

            throw new Exception("Request type not supported");
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
    }
}
