#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Validation.Validators
{
    /// <summary>
    /// Less than validator.
    /// Checks whether the property value is less than valueToCompare
    /// </summary>
    public class LessThanValidator : AbstractComparisonValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="valueToCompare">Value to compare against</param>
        /// <param name="errorMessage">Error message when validation fails</param>
        public LessThanValidator(
            IComparable valueToCompare,
            string errorMessage)
            : base(valueToCompare, errorMessage)
        { }

        /// <summary>
        /// Returns whether or not the property value is less than valueToCompare
        /// </summary>
        /// <param name="value">The property value</param>
        /// <param name="valueToCompare">Value to compare against</param>
        /// <returns>True when value is less than valueToCompare; false, otherwise</returns>
        protected override bool IsValid(
            IComparable value,
            IComparable valueToCompare)
        {
            return value.CompareTo(valueToCompare) < 0;
        }
    }
}
