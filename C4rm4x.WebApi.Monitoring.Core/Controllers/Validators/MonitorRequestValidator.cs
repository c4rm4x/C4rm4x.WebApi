#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Validation;
using C4rm4x.WebApi.Validation.Validators;

#endregion

namespace C4rm4x.WebApi.Monitoring.Core.Controllers
{
    /// <summary>
    /// Monitor request validator
    /// </summary>
    public class MonitorRequestValidator :
            AbstractValidator<MonitorRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MonitorRequestValidator()
        {
            RuleFor(r => r.Components)
                .SetValidator(GetComponentsCollectionValidator())
                .When(r => !r.Components.IsNullOrEmpty());
        }

        private static IPropertyValidator GetComponentsCollectionValidator()
        {
            return new CollectionValidator<ComponentDto>(
                () => new ComponentDtoValidator());
        }
    }
}