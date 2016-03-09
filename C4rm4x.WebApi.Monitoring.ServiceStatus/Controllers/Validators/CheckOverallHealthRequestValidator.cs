#region Using

using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts;
using C4rm4x.WebApi.Validation;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Validators
{
    internal class CheckOverallHealthRequestValidator : 
        AbstractValidator<CheckOverallHealthRequest>
    {
        public CheckOverallHealthRequestValidator()
        {
            // Validates nothing 
        }
    }
}
