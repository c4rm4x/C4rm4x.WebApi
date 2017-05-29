#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Validation.Core;
using System;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation.Validators
{
    #region Interface

    /// <summary>
    /// Checks whether or not the property value fulfills the predicate
    /// </summary>
    public interface IPredicateValidator<TProperty> : IPropertyValidator
    {
        /// <summary>
        /// The predicate that must be true
        /// </summary>
        Func<TProperty, Task<bool>> PredicateAsync { get; }
    }

    #endregion

    /// <summary>
    /// Predicate validator.
    /// Checks whether or not the property value fulfills the predicate
    /// </summary>
    public class PredicateValidator<TProperty> :
        PropertyValidator, IPredicateValidator<TProperty>
    {
        /// <summary>
        /// The predicate that must be true
        /// </summary>       
        public Func<TProperty, Task<bool>> PredicateAsync { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="predicate">The predicate that must be true</param>
        /// <param name="errorMessage">Error message to show when validation fails</param>
        public PredicateValidator(
            Func<TProperty, Task<bool>> predicate,
            string errorMessage) 
            : base(errorMessage)
        {
            predicate.NotNull(nameof(predicate));

            PredicateAsync = predicate;
        }

        /// <summary>
        /// Returns whether or not the property value fulfills the predicate
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override Task<bool> IsValidAsync(PropertyValidatorContext context)
        {
            var value = (TProperty)context.PropertyValue;

            return PredicateAsync(value);
        }
    }
}
