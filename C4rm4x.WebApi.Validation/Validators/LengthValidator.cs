#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Validation.Core;

#endregion

namespace C4rm4x.WebApi.Validation.Validators
{
    #region Interface

    /// <summary>
    /// Lenth validator.
    /// Checks whether the property value lenght is between MinimiumLength and MaximumLength
    /// </summary>
    public interface ILengthValidator : IPropertyValidator
    {
        /// <summary>
        /// The minimum length
        /// </summary>
        int MinimunLength { get; }

        /// <summary>
        /// The maximum length
        /// </summary>
        int MaximumLength { get; }
    }

    #endregion

    /// <summary>
    /// Implementation of ILengthValidator
    /// </summary>
    public class LengthValidator :
        PropertyValidator, ILengthValidator
    {
        /// <summary>
        /// Gets the minimium length
        /// </summary>
        public int MinimunLength { get; private set; }

        /// <summary>
        /// Gets the maximum length
        /// </summary>
        public int MaximumLength { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="minimunLength">The minimun length</param>
        /// <param name="maximumLenght">The maximum length</param>
        /// <param name="errorMessage">Error message to show when validation fails</param>
        public LengthValidator(
            int minimunLength,
            int maximumLenght,
            string errorMessage)
            : base(errorMessage)
        {
            maximumLenght.Must(
                (x) => x == -1 || x >= minimunLength,
                "maximumLenght should be greather than minimumLenght");

            MinimunLength = minimunLength;
            MaximumLength = maximumLenght;
        }

        /// <summary>
        /// Returns whether or not the property value length is between
        /// minimumLength and maximumLength
        /// </summary>
        /// <param name="context">The context</param>
        /// <returns>
        /// True when property value length is between minimum length and maximum length;
        /// false, otherwise</returns>
        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue.IsNull() || !(context.PropertyValue is string))
                return true;

            var length = context.PropertyValue.ToString().Length;

            return length >= MinimunLength &&
                (MaximumLength == -1 || length <= MaximumLength);
        }
    }
}
