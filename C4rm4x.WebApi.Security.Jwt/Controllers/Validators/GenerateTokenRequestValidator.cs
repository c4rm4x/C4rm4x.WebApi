#region Using

using C4rm4x.WebApi.Validation;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Controllers
{
    /// <summary>
    /// Generate token request validator
    /// </summary>
    public class GenerateTokenRequestValidator :
        AbstractValidator<GenerateTokenRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenerateTokenRequestValidator()
        {
            RuleFor(r => r.UserIdentifier).NotEmpty();
        }
    }
}
