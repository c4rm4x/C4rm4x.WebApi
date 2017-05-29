#region Using

using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Validation.Core;
using C4rm4x.WebApi.Validation.Validators;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation
{
    /// <summary>
    /// Defines a rule associated with a property which may have multiple validators
    /// </summary>
    public interface IValidationRule
    {
        /// <summary>
        /// The validators that are grouped under this rule.
        /// </summary>
        IEnumerable<IPropertyValidator> Validators { get; }

        /// <summary>
        /// Name of the rule-set to which this rule belongs.
        /// </summary>
        string RuleSet { get; set; }

        /// <summary>
        /// Performs validation using a validation context and returns a collection of Validation Failures.
        /// </summary>
        /// <param name="context">Validation context</param>
        /// <returns>A collection of validation errors</returns>
        Task<IEnumerable<ValidationError>> ValidateAsync(ValidationContext context);
    }
}
