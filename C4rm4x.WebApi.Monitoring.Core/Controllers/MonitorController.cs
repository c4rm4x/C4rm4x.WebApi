#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Log;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IHttpActionResult> Monitor([FromUri]MonitorRequest request)
        {
            try
            {
                var errors = Validate(request);

                if (errors.Any())
                    return BadRequest(errors);

                return await HandleAsync(request);
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
        protected virtual List<ValidationError> Validate(MonitorRequest request)
        {
            return GetValidator().Validate(request);
        }

        private static IValidator<MonitorRequest> GetValidator()
        {
            return new MonitorRequestValidator();
        }

        private async Task<IHttpActionResult> HandleAsync(MonitorRequest request)
        {
            var results = request.Components.IsNullOrEmpty()
                    ? await HandleOverallSystemAsync(request)
                    : await HandleByComponentAsync(request);

            return Ok(new MonitorResponse<TResult>(results.ToList()));
        }

        private async Task<IEnumerable<TResult>> HandleByComponentAsync(MonitorRequest request)
        {
            var result = new List<TResult>();

            foreach (var component in request.Components)
                result.Add(await GetMonitorResultAsync(component));

            return result;
        }

        private async Task<TResult> GetMonitorResultAsync(ComponentDto component)
        {
            var monitorService = _monitorServices
                .FirstOrDefault(r => r.ComponentIdentifier.Equals(component.Identifier));

            if (monitorService.IsNotNull())
                return await GetMonitorResultAsync(component, monitorService);

            return new TResult { Component = component };
        }

        private async Task<IEnumerable<TResult>> HandleOverallSystemAsync(MonitorRequest request)
        {
            var result = new List<TResult>();

            foreach (var monitorService in _monitorServices)
                result.Add(await GetMonitorResultAsync(
                    GetComponent(monitorService), monitorService));

            return result;
        }

        private static ComponentDto GetComponent(
            IMonitorService<TMonitor> monitorService)
        {
            return new ComponentDto(
                monitorService.ComponentIdentifier,
                monitorService.ComponentName);
        }

        private async Task<TResult> GetMonitorResultAsync(
            ComponentDto component,
            IMonitorService<TMonitor> monitorService)
        {
            return _transformer(component, await monitorService.MonitorAsync());
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
            return HandleUnexpectedException(exception);
        }

        private IHttpActionResult HandleUnexpectedException(Exception exception)
        {
            _logger.Error("Unexpected exception", exception);

            return InternalServerError(
                new Exception("Unexpected server error. Please try again."));
        }

        private static IHttpActionResult BadRequest(List<ValidationError> errors)
        {
            return new BadRequestResult(errors);
        }

        private new static IHttpActionResult InternalServerError(Exception exception)
        {
            return new InternalServerErrorResult<Exception>(exception);
        }
    }
}
