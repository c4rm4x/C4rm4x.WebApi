#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Log;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Monitoring.Core.Controllers
{
    /// <summary>
    /// Basic implementation of an ApiController responsible for monitoring your system
    /// </summary>
    /// <typeparam name="TMonitor">Type of monitor service result</typeparam>
    /// <typeparam name="TResult">Type of monitor response</typeparam>
    public abstract class MonitorController<TMonitor, TResult> :
        ApiController
        where TResult : MonitorResultDto, new()
    {
        private readonly ILog _logger;
        private readonly IEnumerable<IMonitorService<TMonitor>>
            _monitorServices;
        private readonly Func<ComponentDto, TMonitor, TResult>
            _transformer;

        /// <summary>
        /// Controller
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="monitorServices">The collection of all services that implement interface IMonitorService/<TMonitor/></param>
        /// <param name="transformer">Function responsible of generating the response from the component and the result of the monitorization of it</param>
        public MonitorController(
            ILog logger,
            IEnumerable<IMonitorService<TMonitor>> monitorServices,
            Func<ComponentDto, TMonitor, TResult> transformer)
        {
            logger.NotNull(nameof(logger));
            monitorServices.NotNullOrEmpty(nameof(monitorServices));
            transformer.NotNull(nameof(transformer));

            _logger = logger;
            _monitorServices = monitorServices;
            _transformer = transformer;
        }

        /// <summary>
        /// Monitors your system (or a collection of specific components within)
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>An instance of MonitorResponse with result of monitoring your system (or a collection of specific components within)</returns>
        [HttpGet]
        public IHttpActionResult Monitor([FromUri]MonitorRequest request)
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

        /// <summary>
        /// Validates the request
        /// </summary>
        /// <param name="request">The request</param>
        /// <exception cref="ValidationException">If request is not valid</exception>
        protected virtual void Validate(MonitorRequest request)
        {
            GetValidator()
                .ThrowIf(request);
        }

        private static IValidator<MonitorRequest> GetValidator()
        {
            return new MonitorRequestValidator();
        }

        private IHttpActionResult Handle(MonitorRequest request)
        {
            var results = request.Components.IsNullOrEmpty()
                    ? HandleOverallSystem(request)
                    : HandleByComponent(request);

            return Ok(new MonitorResponse<TResult>(results.ToList()));
        }

        private IEnumerable<TResult> HandleByComponent(MonitorRequest request)
        {
            foreach (var component in request.Components)
                yield return GetMonitorResult(component);
        }

        private TResult GetMonitorResult(ComponentDto component)
        {
            var monitorService = _monitorServices
                .FirstOrDefault(r => r.ComponentIdentifier.Equals(component.Identifier));

            if (monitorService.IsNotNull())
                return GetMonitorResult(component, monitorService);

            return new TResult { Component = component };
        }

        private IEnumerable<TResult> HandleOverallSystem(MonitorRequest request)
        {
            foreach (var monitorService in _monitorServices)
                yield return GetMonitorResult(
                    GetComponent(monitorService), monitorService);
        }

        private static ComponentDto GetComponent(
            IMonitorService<TMonitor> monitorService)
        {
            return new ComponentDto(
                monitorService.ComponentIdentifier,
                monitorService.ComponentName);
        }

        private TResult GetMonitorResult(
            ComponentDto component,
            IMonitorService<TMonitor> monitorService)
        {
            return _transformer(component, monitorService.Monitor());
        }

        private static IHttpActionResult Ok(MonitorResponse<TResult> response)
        {
            return new OkResult<MonitorResponse<TResult>>(response);
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
