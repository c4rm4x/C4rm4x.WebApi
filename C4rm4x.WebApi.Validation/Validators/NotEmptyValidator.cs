#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Validation.Core;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation.Validators
{
    #region Interface

    /// <summary>
    /// Not empty validator.
    /// Checks whether the property value is empty
    /// </summary>
    public interface INotEmptyValidator : IPropertyValidator { }

    #endregion

    /// <summary>
    /// Implementation of INotEmptyValidator
    /// </summary>
    public class NotEmptyValidator : PropertyValidator, INotEmptyValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorMessage">Error message to show when validation fails</param>
        public NotEmptyValidator(string errorMessage) :
            base(errorMessage)
        { }

        /// <summary>
        /// Returns whether or not the property value is not empty
        /// </summary>
        /// <param name="context">The context</param>
        /// <returns>True when property value is not empty; false, otherwise</returns>
        protected override Task<bool> IsValidAsync(PropertyValidatorContext context)
        {
            var result = context.PropertyValue.IsNull() ||
                IsEmptyString(context) ||
                IsEmptyCollection(context);

            return Task.FromResult(!result);
        }

        private bool IsEmptyString(PropertyValidatorContext context)
        {
            if (context.PropertyValue is string)
                return string.IsNullOrWhiteSpace(context.PropertyValue as string);

            return false;
        }

        private bool IsEmptyCollection(PropertyValidatorContext context)
        {
            var collection = context.PropertyValue as IEnumerable;

            return collection != null && !collection.Cast<object>().Any();
        }
    }
}
