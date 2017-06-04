#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Validation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation.Validators
{
    #region Interface

    /// <summary>
    /// Collection validator
    /// Checks whether all the elements of the collection satisfy the inner validation
    /// </summary>
    /// <typeparam name="T">The type of the elements of the collection</typeparam>
    public interface ICollectionValidator<T> :
        IPropertyValidator
    {
        /// <summary>
        /// Gets the function that returns the inner validator
        /// </summary>
        Func<IValidator<T>> Validator { get; }
    }

    #endregion

    /// <summary>
    /// Collection validator
    /// Checks whether all the elements of the collection satisfy the inner validation
    /// </summary>
    /// <typeparam name="T">The type of the elements of the collection</typeparam>
    public class CollectionValidator<T> :
        ICollectionValidator<T>
    {
        /// <summary>
        /// Gets the function that returns the inner validator
        /// </summary>
        public Func<IValidator<T>> Validator { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validator">A function that returns the inner validation</param>
        public CollectionValidator(
            Func<IValidator<T>> validator)
        {
            validator.NotNull(nameof(validator));

            Validator = validator;
        }

        /// <summary>
        /// Returns all the validation errors for the specified context
        /// </summary>
        /// <param name="context">The context to validate</param>
        /// <returns>List of all validation errors</returns>
        public Task<IEnumerable<ValidationError>> ValidateAsync(
            PropertyValidatorContext context)
        {
            if (context.PropertyValue is IEnumerable<T>)
                return GetValidationErrorsAsync(context.PropertyValue as IEnumerable<T>);

            return Task.FromResult(Enumerable.Empty<ValidationError>());
        }

        private async Task<IEnumerable<ValidationError>> GetValidationErrorsAsync(
            IEnumerable<T> items)
        {
            var results = await Task.WhenAll(items.Select(
                item => Validator().ValidateAsync(item)));

            return results.SelectMany(result => result);
        }
    }
}
