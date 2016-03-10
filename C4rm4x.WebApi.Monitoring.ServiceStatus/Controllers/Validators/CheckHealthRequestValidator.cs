#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos;
using C4rm4x.WebApi.Validation;
using C4rm4x.WebApi.Validation.Validators;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Validators
{
    internal class CheckHealthRequestValidator : 
        AbstractValidator<CheckHealthRequest>
    {
        public CheckHealthRequestValidator()
        {
            RuleFor(r => r.Components)
                .SetValidator(GetComponentsCollectionValidator())
                .When(r => !r.Components.IsNullOrEmpty());
        }

        private static IPropertyValidator GetComponentsCollectionValidator()
        {
            return new CollectionValidator<ComponentDto>(
                () => new ComponentDtoValidator(),
                "Components is not a collection of valid ComponentDto's");
        }
    }    
}
