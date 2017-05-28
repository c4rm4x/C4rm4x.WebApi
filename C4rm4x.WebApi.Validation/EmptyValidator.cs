#region Using

using C4rm4x.WebApi.Framework.Validation;
using System;
using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Validation
{
    internal class EmptyValidator : IValidator
    {
        public bool CanValidateInstancesOf(Type type)
        {
            return true;
        }

        public List<ValidationError> Validate(object objectToValidate)
        {
            return new List<ValidationError>();
        }

        public List<ValidationError> Validate(
            object objectToValidate, 
            string ruleSetName)
        {
            return Validate(objectToValidate);
        }
    }
}
