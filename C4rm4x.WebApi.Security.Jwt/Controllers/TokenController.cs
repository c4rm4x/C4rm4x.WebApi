#region Using

using C4rm4x.Tools.Utilities;
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
        private readonly IClaimsIdentityRetriever _claimsIdentityRetriever;
        private readonly IJwtSecurityTokenGenerator _jwtSecurityTokenGenerator;
        private readonly IJwtGenerationOptionsFactory _jwtGenerationOptionsFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="claimsIdentityRetriever">The implementation to retrieve ClaimsIdentity</param>
        /// <param name="jwtSecurityTokenGenerator">The JWT token generator</param>
        /// <param name="jwtGenerationOptionsFactory">The Jwt generation options factory</param>
        public TokenController(
            IClaimsIdentityRetriever claimsIdentityRetriever,
            IJwtSecurityTokenGenerator jwtSecurityTokenGenerator,
            IJwtGenerationOptionsFactory jwtGenerationOptionsFactory)
        {
            claimsIdentityRetriever.NotNull(nameof(claimsIdentityRetriever));
            jwtSecurityTokenGenerator.NotNull(nameof(jwtSecurityTokenGenerator));
            jwtGenerationOptionsFactory.NotNull(nameof(jwtGenerationOptionsFactory));

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

                return Ok(RetrieveToken(request));
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
            if (request.UserIdentifier.IsNullOrEmpty())
                throw new ValidationException("UserIdentifier: Cannot be null or empty");
        }

        private GenerateTokenResponse RetrieveToken(GenerateTokenRequest request)
        {
            var claimsIdentity = _claimsIdentityRetriever
                .Retrieve(request.UserIdentifier, request.Secret);

            if (claimsIdentity.IsNull())
                throw new UserCredentialsException();

            return new GenerateTokenResponse(
                _jwtSecurityTokenGenerator.Generate(claimsIdentity, GetOptions()));
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
