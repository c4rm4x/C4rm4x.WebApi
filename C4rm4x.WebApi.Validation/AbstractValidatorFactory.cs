﻿#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Validation.Validators;
using System;

#endregion

namespace C4rm4x.WebApi.Validation
{
    /// <summary>
    /// Base implmentation of IValidatorFactory
    /// </summary>
    public abstract class AbstractValidatorFactory : IValidatorFactory
    {
        /// <summary>
        /// Gets the validator for the specified type
        /// </summary>
        public IValidator<T> GetValidator<T>()
        {
            return (IValidator<T>)GetValidator(typeof(T));
        }

        /// <summary>
        /// Gets the validator for the specified type (or any base class for which the validator is defined)
        /// </summary>
        public IValidator GetValidator(Type type)
        {
            if (type == typeof(object))
                throw new ArgumentException("There is no valid validator for the type System.Object");

            IValidator validator = null;

            do
            {
                validator = CreateInstance(type);
            }
            while (validator.IsNull() && (type = type.BaseType).IsNotNull() && type != typeof(object));

            return validator ?? new EmptyValidator();
        }

        /// <summary>
        /// Retrieves the instance of IValidator for the specified type
        /// </summary>
        /// <param name="type">The type to validate</param>
        /// <returns>The instance of IValidator for the specifid type (if any)</returns>
        protected abstract IValidator CreateInstance(Type type);
    }
}
