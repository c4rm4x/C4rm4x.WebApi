namespace C4rm4x.WebApi.Validation.Core
{
    internal class DefaultValidatorSelector : IValidatorSelector
    {
        public bool CanExecute(
            IValidationRule rule,
            string propertyPath,
            ValidationContext context)
        {
            return string.IsNullOrEmpty(rule.RuleSet);
        }
    }
}
