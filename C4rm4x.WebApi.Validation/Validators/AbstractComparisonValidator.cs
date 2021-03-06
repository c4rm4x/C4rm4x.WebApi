﻿#region Using

using C4rm4x.WebApi.Validation.Core;
using System;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation.Validators
{
    #region Interface

    /// <summary>
    /// Comparison validator.
    /// Checks whether the property value is comparable somehow with valueToCompare
    /// </summary>
    public interface IComparisonValidator : IPropertyValidator
    {
        /// <summary>
        /// Value to compare against
        /// </summary>
        IComparable ValueToCompare { get; }
    }

    #endregion

    /// <summary>
    /// Base class that implements IComparisonValidator
    /// </summary>
    public abstract class AbstractComparisonValidator :
        PropertyValidator, IComparisonValidator
    {
        /// <summary>
        /// Gets the value to compare against
        /// </summary>
        public IComparable ValueToCompare { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="valueToCompare">Value to compare against</param>
        /// <param name="errorMessage">Error message to show when validation fails</param>
        public AbstractComparisonValidator(
            IComparable valueToCompare,
            string errorMessage)
            : base(errorMessage)
        {
            ValueToCompare = valueToCompare;
        }

        /// <summary>
        /// Returns whether or not the property value is valid comparing somehow
        /// with valueToCompare
        /// </summary>
        /// <param name="context">The context</param>
        /// <returns>True if property value is valid; false, otherwise</returns>
        protected override Task<bool> IsValidAsync(PropertyValidatorContext context)
        {
            return IsValidAsync((IComparable)context.PropertyValue, ValueToCompare);
        }

        /// <summary>
        /// Returns whether or not property value is valid comparing somehow
        /// with valueToCompare
        /// </summary>
        /// <param name="value">The property value</param>
        /// <param name="valueToCompare">Value to compare against</param>
        /// <returns>True if property value is valid; false, otherwise</returns>
        protected abstract Task<bool> IsValidAsync(
            IComparable value,
            IComparable valueToCompare);
    }
}
