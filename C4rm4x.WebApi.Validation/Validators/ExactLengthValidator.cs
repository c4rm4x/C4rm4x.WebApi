namespace C4rm4x.WebApi.Validation.Validators
{
    /// <summary>
    /// Exact length validator.
    /// Checks whether or not the property value size is exactly length
    /// </summary>
    public class ExactLengthValidator : LengthValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="length">The exact length</param>
        /// <param name="errorMessage">Error to show when validation fails</param>
        public ExactLengthValidator(
            int length,
            string errorMessage)
            : base(length, length, errorMessage)
        { }
    }
}
