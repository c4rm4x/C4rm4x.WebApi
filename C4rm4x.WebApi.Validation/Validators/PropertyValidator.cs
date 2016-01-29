#region Using

using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Validation.Core;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Validation.Validators
{
    #region Interface

    /// <summary>
    /// A custom property validator
    /// </summary>
    public interface IPropertyValidator
    {
        /// <summary>
        /// Performs validation using a property validator context and returns a collection of Validation Failures.
        /// </summary>
        /// <param name="context">Property validator context</param>
        /// <returns>A collection of validation errors</returns>
        IEnumerable<ValidationError> Validate(PropertyValidatorContext context);

        /// <summary>
        /// Error message to show when validation fails
        /// </summary>
        string ErrorMessage { get; }
    }

    #endregion

    /// <summary>
    /// Base implementation of IPropertyValidator
    /// </summary>
    public abstract class PropertyValidator : IPropertyValidator
    {
        /// <summary>
        /// Gets the error message to show when validation fails
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        public PropertyValidator(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Returns all the validation errors for the specified context
        /// </summary>
        /// <param name="context">The context to validate</param>
        /// <returns>Lit of all validation errors</returns>
        public IEnumerable<ValidationError> Validate(
            PropertyValidatorContext context)
        {
            if (!IsValid(context))
                return new[] { CreateValidationError(context) };

            return Enumerable.Empty<ValidationError>();
        }

        /// <summary>
        /// Returns whether or not the property validator context is valid
        /// </summary>
        /// <param name="context">The context</param>
        /// <returns>True if context is valid; false, otherwise</returns>
        protected abstract bool IsValid(PropertyValidatorContext context);

        /// <summary>
        /// Returns an instance of ValidationError
        /// </summary>
        /// <param name="context">The context</param>
        /// <returns>An instance of ValidationError</returns>
        protected virtual ValidationError CreateValidationError(
            PropertyValidatorContext context)
        {
            return new ValidationError(context.PropertyName, context.PropertyValue, ErrorMessage);
        }
    }
}
