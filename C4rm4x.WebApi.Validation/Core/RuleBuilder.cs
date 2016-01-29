#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Validation.Validators;
using System;

#endregion

namespace C4rm4x.WebApi.Validation.Core
{
    #region Interface

    /// <summary>
    /// Builds a conditional validator rule and constructr a validator
    /// </summary>
    /// <typeparam name="T">Type of object being validated</typeparam>
    /// <typeparam name="TProperty">Type of property being validated</typeparam>
    public interface IRuleBuilderOptions<T, out TProperty>
    {
        /// <summary>
        /// Sets the condition
        /// </summary>
        /// <param name="applyCondition">The predicate</param>
        IRuleBuilderOptions<T, TProperty> Configure(
            Func<T, bool> applyCondition);
    }

    /// <summary>
    /// Builds a validator rule and constructs a validator
    /// </summary>
    /// <typeparam name="T">Type of object being validated</typeparam>
    /// <typeparam name="TProperty">Type of property being validated</typeparam>
    public interface IRuleBuilder<T, out TProperty> :
        IRuleBuilderOptions<T, TProperty>
    {
        /// <summary>
        /// Associates a validator with this the property for this rule builder.
        /// </summary>
        /// <param name="validator">The validator to set</param>
        /// <returns>RuleBuilderOptions for the property</returns>
        IRuleBuilder<T, TProperty> SetValidator(IPropertyValidator validator);
    }

    #endregion

    /// <summary>
    /// Builds a validation rule and constructs a validator
    /// </summary>
    /// <typeparam name="T">Type of object being validated</typeparam>
    /// <typeparam name="TProperty">Type of property being validated</typeparam>
    internal class RuleBuilder<T, TProperty> : IRuleBuilder<T, TProperty>
    {
        /// <summary>
        /// The rule being created by this RuleBuilder
        /// </summary>
        public PropertyRule Rule { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rule">The rule</param>
        public RuleBuilder(PropertyRule rule)
        {
            Rule = rule;
        }

        /// <summary>
        /// Associates a validator with this the property for this rule builder.
        /// </summary>
        /// <param name="validator">The validator to set</param>
        /// <returns>RuleBuilderOptions for the property</returns>
        public IRuleBuilder<T, TProperty> SetValidator(IPropertyValidator validator)
        {
            validator.NotNull(nameof(validator));

            Rule.AddValidator(validator);

            return this;
        }

        /// <summary>
        /// Sets the condition
        /// </summary>
        /// <param name="applyCondition">The predicate</param>
        public IRuleBuilderOptions<T, TProperty> Configure(
            Func<T, bool> applyCondition)
        {
            applyCondition.NotNull(nameof(applyCondition));

            Rule.ApplyCondition(x => applyCondition((T)x));

            return this;
        }
    }
}
