namespace C4rm4x.WebApi.Validation.Core
{
    /// <summary>
    /// Determines whether or not a rule should execute.
    /// </summary>
    public interface IValidatorSelector
    {
        /// <summary>
        /// Determines whether or not a rule should be executed
        /// </summary>
        /// <param name="rule">The rule</param>
        /// <param name="propertyPath">Property path</param>
        /// <param name="context">Contextual information</param>
        /// <returns>True if the validator can be executed. False otherwise</returns>
        bool CanExecute(
            IValidationRule rule,
            string propertyPath,
            ValidationContext context);
    }
}
