#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts;
using C4rm4x.WebApi.Validation;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Validators
{
    internal class CheckComponentsHealthRequestValidator : 
        AbstractValidator<CheckComponentsHealthRequest>
    {
        public CheckComponentsHealthRequestValidator()
        {
            RuleFor(r => r.Components).NotEmpty();
            RuleFor(r => r.Components)
                .SetValidator(new CollectionComponentDtoValidator())
                .When(r => !r.Components.IsNullOrEmpty());
        }
    }    
}
