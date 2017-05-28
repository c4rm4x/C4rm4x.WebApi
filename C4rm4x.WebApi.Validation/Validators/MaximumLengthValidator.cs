namespace C4rm4x.WebApi.Validation.Validators
{
    /// <summary>
    /// Maximum length validator.
    /// Checks whether the property value length is more than maximumLength
    /// </summary>
    public class MaximumLengthValidator : LengthValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maximumLength">The maximum length</param>
        /// <param name="errorMessage">Error message when validation fails</param>
        public MaximumLengthValidator(
            int maximumLength,
            string errorMessage)
            : base(0, maximumLength, errorMessage)
        { }
    }
}
