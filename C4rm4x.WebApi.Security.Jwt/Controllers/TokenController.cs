#region Using

using C4rm4x.Tools.Security.Jwt;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Log;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Controllers
{
    /// <summary>
    /// Basic implementation of an ApiController responsible for generating Json Web Tokens
    /// </summary>
    public class TokenController : ApiController
    {
        private readonly ILog _logger;
        private readonly IClaimsIdentityRetriever _claimsIdentityRetriever;
        private readonly IJwtSecurityTokenGenerator _jwtSecurityTokenGenerator;
        private readonly IJwtGenerationOptionsFactory _jwtGenerationOptionsFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="claimsIdentityRetriever">The implementation to retrieve ClaimsIdentity</param>
        /// <param name="jwtSecurityTokenGenerator">The JWT token generator</param>
        /// <param name="jwtGenerationOptionsFactory">The Jwt generation options factory</param>
        public TokenController(
            ILog logger,
            IClaimsIdentityRetriever claimsIdentityRetriever,
            IJwtSecurityTokenGenerator jwtSecurityTokenGenerator,
            IJwtGenerationOptionsFactory jwtGenerationOptionsFactory)
        {
            logger.NotNull(nameof(logger));
            claimsIdentityRetriever.NotNull(nameof(claimsIdentityRetriever));
            jwtSecurityTokenGenerator.NotNull(nameof(jwtSecurityTokenGenerator));
            jwtGenerationOptionsFactory.NotNull(nameof(jwtGenerationOptionsFactory));

            _logger = logger;
            _claimsIdentityRetriever = claimsIdentityRetriever;
            _jwtSecurityTokenGenerator = jwtSecurityTokenGenerator;
            _jwtGenerationOptionsFactory = jwtGenerationOptionsFactory;
        }

        /// <summary>
        /// Generates a token for the specified user (if found)
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>An instance of GenerateTokenResponse with the token generated (if successfully)</returns>
        [HttpPost]
        public async Task<IHttpActionResult> GenerateToken(GenerateTokenRequest request)
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
        protected virtual Task<List<ValidationError>> ValidateAsync(GenerateTokenRequest request)
        {
            return GetValidator().ValidateAsync(request);
        }

        private GenerateTokenRequestValidator GetValidator()
        {
            return new GenerateTokenRequestValidator();
        }

        private async Task<IHttpActionResult> HandleAsync(GenerateTokenRequest request)
        {
            var claimsIdentity = await _claimsIdentityRetriever
                .RetrieveAsync(request.UserIdentifier, request.Secret);

            if (claimsIdentity.IsNull())
                return Unauthorized(new AuthenticationHeaderValue("Basic"));

            var response = new GenerateTokenResponse(
                _jwtSecurityTokenGenerator.Generate(claimsIdentity, GetOptions()));

            return Ok(response);
        }

        private JwtGenerationOptions GetOptions()
        {
            return _jwtGenerationOptionsFactory.GetOptions();
        }

        private static IHttpActionResult Ok(GenerateTokenResponse response)
        {
            return new OkResult<GenerateTokenResponse>(response);
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
