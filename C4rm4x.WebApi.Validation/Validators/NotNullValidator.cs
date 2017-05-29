#region Using

using C4rm4x.WebApi.Validation.Core;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation.Validators
{
    #region Interface

    /// <summary>
    /// Not null validator
    /// Checks whether the property value is null
    /// </summary>
    public interface INotNullValidator : IPropertyValidator { }

    #endregion

    /// <summary>
    /// Implementation of INotNullValidator
    /// </summary>
    public class NotNullValidator : PropertyValidator, INotNullValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorMessage">Error to show when validation fails</param>
        public NotNullValidator(string errorMessage)
            : base(errorMessage)
        { }

        /// <summary>
        /// Returns whether or not the property is valid based on nullability condition
        /// </summary>
        /// <param name="context">The property context</param>
        /// <returns></returns>
        protected override Task<bool> IsValidAsync(PropertyValidatorContext context)
        {
            return Task.FromResult(context.PropertyValue != null);
        }
    }
}
