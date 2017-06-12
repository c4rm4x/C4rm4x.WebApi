#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Log;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Configuration.Controllers
{
    /// <summary>
    /// Basic implementation of an ApiController responsible to retrieve App configuration for given user
    /// </summary>
    public class ConfigurationController : ApiController
    {
        private readonly ILog _logger;

        private readonly IAppConfigurationRepository _appConfigurations;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="appConfigurations">The app configuration repository</param>
        public ConfigurationController(
            ILog logger,
            IAppConfigurationRepository appConfigurations)
        {
            logger.NotNull(nameof(logger));
            appConfigurations.NotNull(nameof(appConfigurations));

            _logger = logger;
            _appConfigurations = appConfigurations;
        }
            
        /// <summary>
        /// Retrieve the configuration for the given app-user
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>An instance of GetConfigurationResponse with the collection of endpoints this app-user has access to</returns>
        [HttpGet]
        [Secured]
        public async Task<IHttpActionResult> Retrieve(
            [FromUri]GetConfigurationRequest request)
        {
            try
            {
                var errors = await ValidateAsync(request);

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
        /// <param name="request">The request to validate</param>
        protected virtual Task<List<ValidationError>> ValidateAsync(GetConfigurationRequest request)
        {
            return GetValidator().ValidateAsync(request);
        }

        private GetConfigurationRequestValidator GetValidator()
        {
            return new GetConfigurationRequestValidator();
        }

        private async Task<IHttpActionResult> HandleAsync(GetConfigurationRequest request)
        {
            var configuration = await _appConfigurations
                .GetConfigurationAsync(request.AppIdentifier, request.Version);

            if (configuration.IsNull()) return NotFound();

            return Ok(RetrieveConfigurationResponse(configuration));
        }

        /// <summary>
        /// Returns the configuration for the app/version/user
        /// </summary>
        /// <param name="configuration">The configuration</param>
        /// <returns></returns>
        protected virtual GetConfigurationResponse RetrieveConfigurationResponse(AppConfiguration configuration)
        {
            return new GetConfigurationResponse
            {
                Endpoints = GetActualEndpoints(configuration.Endpoints)
            };
        }

        private IEnumerable<EndpointDto> GetActualEndpoints(IEnumerable<Endpoint> endpoints)
        {
            return endpoints?.Where(IsAuthorized).Select(e => new EndpointDto
            {
                Context = e.Context,
                Url = e.Url
            });
        }

        private bool IsAuthorized(Endpoint endpoint)
        {
            if (!endpoint.Permissions.Any()) return true;

            var princial = RequestContext.Principal as ClaimsPrincipal;

            if (endpoint.Permissions.Any(permission => permission.IsAuthorized(princial))) return true;

            return false;
        }

        // <summary>
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

        private static IHttpActionResult Ok(GetConfigurationResponse response)
        {
            return new OkResult<GetConfigurationResponse>(response);
        }

        private new static IHttpActionResult InternalServerError(Exception exception)
        {
            return new InternalServerErrorResult<Exception>(exception);
        }
    }
}
