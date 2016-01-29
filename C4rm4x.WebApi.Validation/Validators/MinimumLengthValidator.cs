namespace C4rm4x.WebApi.Validation.Validators
{
    /// <summary>
    /// Minimum length validator. 
    /// Checks whether or not the length of the property value is less than minimumLength
    /// </summary>
    public class MinimumLengthValidator : LengthValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="minimumLength">The minimun length</param>
        /// <param name="errorMessage">Error to show when validation fails</param>
        public MinimumLengthValidator(
            int minimumLength,
            string errorMessage)
            : base(minimumLength, -1, errorMessage)
        { }
    }
}
