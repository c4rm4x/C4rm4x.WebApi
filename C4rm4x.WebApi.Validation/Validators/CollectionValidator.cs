#region Using

using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Validation.Core;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Validation.Validators
{
    #region Interface

    /// <summary>
    /// Collection validation.
    /// Checks whether all the elements of the collection satisfy the inner validation
    /// </summary>
    /// <typeparam name="T">The type of the elements of the collection</typeparam>
    public interface ICollectionValidator<T> : IPropertyValidator
    {
        /// <summary>
        /// Gets a function that returns the inner validator
        /// </summary>
        Func<IValidator<T>> Validator { get; }
    }

    #endregion

    /// <summary>
    /// Implementation of ICollectionValidator
    /// </summary>
    /// <typeparam name="T">The type of the elements of the collection</typeparam>
    public class CollectionValidator<T> :
        PropertyValidator, ICollectionValidator<T>
    {
        /// <summary>
        /// Gets a function that returns the inner validator
        /// </summary>
        public Func<IValidator<T>> Validator { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validator">A function that returns the inner validator</param>
        /// <param name="errorMessage">Error message to show when validation fails</param>
        public CollectionValidator(
            Func<IValidator<T>> validator,
            string errorMessage)
            : base(errorMessage)
        {
            Validator = validator;
        }

        /// <summary>
        /// Returns whether or not the property value is a collection of valid elements of type T
        /// </summary>
        /// <param name="context">The context</param>
        /// <returns>True when property value is not empty; false, otherwise</returns>
        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue is IEnumerable<T>)
                return !GetValidationErrors(
                    context.PropertyValue as IEnumerable<T>).Any();

            return false;
        }

        private IEnumerable<ValidationError> GetValidationErrors(
            IEnumerable<T> objectsToValidate)
        {
            foreach (var objectToValidate in objectsToValidate)
                foreach (var error in Validator().Validate(objectToValidate))
                    yield return error;
        }
    }
}
