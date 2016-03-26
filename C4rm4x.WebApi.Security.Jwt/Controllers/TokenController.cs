#region Using

using C4rm4x.Tools.Security.Jwt;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Log;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Framework.Validation;
using System;
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
        public IHttpActionResult GenerateToken(GenerateTokenRequest request)
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
        /// <param name="request">The request to validate</param>
        /// <exception cref="ValidationException">If request is not valid</exception>
        protected virtual void Validate(GenerateTokenRequest request)
        {
            GetValidator()
                .ThrowIf(request);
        }

        private GenerateTokenRequestValidator GetValidator()
        {
            return new GenerateTokenRequestValidator();
        }

        private IHttpActionResult Handle(GenerateTokenRequest request)
        {
            var claimsIdentity = _claimsIdentityRetriever
                .Retrieve(request.UserIdentifier, request.Secret);

            if (claimsIdentity.IsNull())
                throw new UserCredentialsException();

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
            if (exception is ValidationException)
                return BadRequest(exception as ValidationException);

            if (exception is UserCredentialsException)
                return InternalServerError(exception);

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
