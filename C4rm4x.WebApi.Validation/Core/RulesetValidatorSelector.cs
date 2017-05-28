namespace C4rm4x.WebApi.Validation.Core
{
    internal class RulesetValidatorSelector : IValidatorSelector
    {
        public string RuleSetName { get; private set; }

        public RulesetValidatorSelector(string ruleSetToExecute)
        {
            RuleSetName = ruleSetToExecute;
        }

        public bool CanExecute(
            IValidationRule rule,
            string propertyPath,
            ValidationContext context)
        {
            if (!string.IsNullOrEmpty(rule.RuleSet) && rule.RuleSet.Equals(RuleSetName))
                return true;

            return false;
        }
    }
}
