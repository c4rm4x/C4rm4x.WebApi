#region Using

using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos;
using C4rm4x.WebApi.Validation;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Validators
{
    internal class ComponentDtoValidator :
        AbstractValidator<ComponentDto>
    {
        public ComponentDtoValidator()
        {
            RuleFor(c => c.Identifier).NotNull();
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
