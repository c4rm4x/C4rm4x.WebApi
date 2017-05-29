#region Using

using System;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation.Validators
{
    /// <summary>
    /// Greather than validator.
    /// Checks whether the property value is greater than valueToCompare
    /// </summary>
    public class GreaterThanValidator : AbstractComparisonValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="valueToCompare">Value to compare against</param>
        /// <param name="errorMessage">Error message to show when validation fails</param>
        public GreaterThanValidator(
            IComparable valueToCompare,
            string errorMessage)
            : base(valueToCompare, errorMessage)
        { }

        /// <summary>
        /// Returns whether or not the property value is greater than valueToCompare
        /// </summary>
        /// <param name="value">The property value</param>
        /// <param name="valueToCompare">Value to compare against</param>
        /// <returns>True if property value is greater than valueToCompare; false, otherwise</returns>
        protected override Task<bool> IsValidAsync(
            IComparable value,
            IComparable valueToCompare)
        {
            return Task.FromResult(value.CompareTo(valueToCompare) > 0);
        }
    }
}
