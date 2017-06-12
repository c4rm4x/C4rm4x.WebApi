#region Using

using C4rm4x.WebApi.Validation;

#endregion

namespace C4rm4x.WebApi.Configuration.Controllers
{
    /// <summary>
    /// Get configuration request validator
    /// </summary>
    public class GetConfigurationRequestValidator :
        AbstractValidator<GetConfigurationRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GetConfigurationRequestValidator()
        {
            RuleFor(e => e.AppIdentifier).NotEmpty();
            RuleFor(e => e.Version).NotEmpty();
        }
    }
}
