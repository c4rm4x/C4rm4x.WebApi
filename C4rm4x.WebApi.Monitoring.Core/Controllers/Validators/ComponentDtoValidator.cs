#region Using

using C4rm4x.WebApi.Validation;

#endregion

namespace C4rm4x.WebApi.Monitoring.Core.Controllers
{
    /// <summary>
    /// Component Dto validator
    /// </summary>
    public class ComponentDtoValidator :
        AbstractValidator<ComponentDto>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ComponentDtoValidator()
        {
            RuleFor(c => c.Identifier).NotNull();
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
