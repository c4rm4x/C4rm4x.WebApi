#region Using

using C4rm4x.WebApi.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation
{
    internal class EmptyValidator : IValidator
    {
        public bool CanValidateInstancesOf(Type type)
        {
            return true;
        }

        public Task<List<ValidationError>> ValidateAsync(object objectToValidate)
        {
            return Task.FromResult(Enumerable.Empty<ValidationError>().ToList());
        }

        public Task<List<ValidationError>> ValidateAsync(
            object objectToValidate, 
            string ruleSetName)
        {
            return ValidateAsync(objectToValidate);
        }
    }
}
