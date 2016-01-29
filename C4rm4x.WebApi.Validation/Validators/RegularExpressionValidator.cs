#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Validation.Core;
using System.Text.RegularExpressions;

#endregion

namespace C4rm4x.WebApi.Validation.Validators
{
    #region Interface

    /// <summary>
    /// Checks whether or not the property value matches the regular expression
    /// </summary>
    public interface IRegularExpressionValidator : IPropertyValidator
    {
        /// <summary>
        /// The regular expresion that propety value must match
        /// </summary>
        string Expression { get; }
    }

    #endregion

    /// <summary>
    /// Regular expression validator.
    /// Checks whether or not the property value matches the regular expression
    /// </summary>
    public class RegularExpressionValidator :
        PropertyValidator, IRegularExpressionValidator
    {
        private readonly Regex _regex;

        /// <summary>
        /// Gets the regular expresion that property value must match
        /// </summary>
        public string Expression { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="expression">Regular expression as string</param>
        /// <param name="errorMessage">Error message to show when validation fails</param>
        public RegularExpressionValidator(
            string expression,
            string errorMessage)
            : this(new Regex(expression), errorMessage)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="regex">Regular expression as Regex</param>
        /// <param name="errorMessage">Error message to show when validation fails</param>
        public RegularExpressionValidator(
            Regex regex,
            string errorMessage)
            : base(errorMessage)
        {
            Expression = regex.ToString();
            _regex = regex;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="expression">Regular expression as string</param>
        /// <param name="options">Regular expression options</param>
        /// <param name="errorMessage">Error message to show when validation fails</param>
        public RegularExpressionValidator(
            string expression,
            RegexOptions options,
            string errorMessage)
            : this(new Regex(expression, options), errorMessage)
        { }

        /// <summary>
        /// Returns whether or not the property value mathes the regular expression
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override bool IsValid(PropertyValidatorContext context)
        {
            return context.PropertyValue.IsNull() ||
                (context.PropertyValue is string && _regex.IsMatch(context.PropertyValue.ToString()));
        }
    }
}
