#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Validation.Validators
{
    /// <summary>
    /// Greater than or equal validator.
    /// Checks whether the property value is greater than or equal to valueToCompare
    /// </summary>
    public class GreaterThanOrEqualValidator : AbstractComparisonValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="valueToCompare">Value to compare against</param>
        /// <param name="errorMessage">Error message to show when validation fails</param>
        public GreaterThanOrEqualValidator(
            IComparable valueToCompare,
            string errorMessage)
            : base(valueToCompare, errorMessage)
        { }

        /// <summary>
        /// Returns whether or not the property value is greater than or equal to valueToCompare
        /// </summary>
        /// <param name="value">The property value</param>
        /// <param name="valueToCompare">Value to compare against</param>
        /// <returns>True if property value is greater than or equal to valueToCompare; false, otherwise</returns>
        protected override bool IsValid(
            IComparable value,
            IComparable valueToCompare)
        {
            return value.CompareTo(valueToCompare) >= 0;
        }
    }
}
