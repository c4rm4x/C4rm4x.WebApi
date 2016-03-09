#region Using

using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos;
using C4rm4x.WebApi.Validation;
using C4rm4x.WebApi.Validation.Core;
using C4rm4x.WebApi.Validation.Validators;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Validators
{
    internal class CollectionComponentDtoValidator : PropertyValidator
    {
        public CollectionComponentDtoValidator()
            : base("Components is not a collection of valid ComponentDto's")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue is IEnumerable<ComponentDto>)
                return !GetValidationErrors(
                    context.PropertyValue as IEnumerable<ComponentDto>).Any();

            return false;
        }

        private IEnumerable<ValidationError> GetValidationErrors(IEnumerable<ComponentDto> componentDtos)
        {
            var validator = new ComponentDtoValidator();

            foreach (var componentDto in componentDtos)
                foreach (var error in validator.Validate(componentDto))
                    yield return error;
        }// Till I have the next version of validator collection
    }

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
