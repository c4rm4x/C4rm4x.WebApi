#region Using

using C4rm4x.WebApi.Validation.Core;
using System.Collections;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation.Validators
{
    #region Interface

    /// <summary>
    /// Equal validator.
    /// Checks whether the property value is equal to valueToCompare
    /// </summary>
    public interface IEqualValidator : IPropertyValidator
    {
        /// <summary>
        /// Value to compare against
        /// </summary>
        object ValueToCompare { get; }
    }

    #endregion

    /// <summary>
    /// Implementation of IEqualValidator
    /// </summary>
    public class EqualValidator :
        PropertyValidator, IEqualValidator
    {
        /// <summary>
        /// Gets the value to compare against
        /// </summary>
        public object ValueToCompare { get; private set; }

        private readonly IEqualityComparer _comparer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="valueToCompare">Value to compare against</param>
        /// <param name="errorMessage">Error message to show when validation fails</param>
        public EqualValidator(
            object valueToCompare,
            string errorMessage)
            : this(valueToCompare, null, errorMessage)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="valueToCompare">Value to compare against</param>
        /// <param name="comparer">Compare equality using this comparer</param>
        /// <param name="errorMessage">Error message to show when validation fails</param>
        public EqualValidator(
            object valueToCompare,
            IEqualityComparer comparer,
            string errorMessage)
            : base(errorMessage)
        {
            ValueToCompare = valueToCompare;
            _comparer = comparer;
        }

        /// <summary>
        /// Returns whether or not the property vaue is equal to valueToCompare
        /// </summary>
        /// <param name="context">The context</param>
        /// <returns>True if property value is equal to valueToCompare; false, otherwise</returns>
        protected override Task<bool> IsValidAsync(PropertyValidatorContext context)
        {
            return Compare(context.PropertyValue, ValueToCompare);
        }

        private Task<bool> Compare(
            object value,
            object valueToCompare)
        {
            return _comparer != null
                ? Task.FromResult(_comparer.Equals(value, valueToCompare))
                : Task.FromResult(value == valueToCompare);
        }
    }
}
