#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Validation.Validators
{
    /// <summary>
    /// Less than or equal validator.
    /// Checks whether the property value is less than or equal to valueToCompare
    /// </summary>
    public class LessThanOrEqualValidator : AbstractComparisonValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="valueToCompare">Value to compare against</param>
        /// <param name="errorMessage">Error message when validation fails</param>
        public LessThanOrEqualValidator(
            IComparable valueToCompare,
            string errorMessage)
            : base(valueToCompare, errorMessage)
        { }

        /// <summary>
        /// Returns whether or not the property value is less than or equal to valueToCompare 
        /// </summary>
        /// <param name="value">The property value</param>
        /// <param name="valueToCompare">Value to compare against</param>
        /// <returns>True if value is less than or equal to valueToCompare; false, otherwise</returns>
        protected override bool IsValid(
            IComparable value,
            IComparable valueToCompare)
        {
            return value.CompareTo(valueToCompare) <= 0;
        }
    }
}
