#region Using

using C4rm4x.WebApi.Validation.Core;
using C4rm4x.WebApi.Validation.Validators;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation
{
    /// <summary>
    /// Utilities methods to configure validator rules with a fluent api
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Defines a 'not null' validator on the current rule builder. 
        /// Validation will fail if the property is null.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        public static IRuleBuilder<T, TProperty> NotNull<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.NotNull("Cannot be null");
        }

        /// <summary>
        /// Defines a 'not null' validator on the current rule builder. 
        /// Validation will fail if the property is null.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="errorMessage">Error message</param>
        public static IRuleBuilder<T, TProperty> NotNull<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            string errorMessage)
        {
            return ruleBuilder.SetValidator(new NotNullValidator(errorMessage));
        }

        /// <summary>
        /// Defines a 'not empty' validator on the current rule builder.
        /// Validation will fail if the property is null, an empty or the default value for the type (for example, 0 for integers)
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        public static IRuleBuilder<T, TProperty> NotEmpty<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.NotEmpty("Cannot be empty");
        }

        /// <summary>
        /// Defines a 'not empty' validator on the current rule builder.
        /// Validation will fail if the property is null, an empty or the default value for the type (for example, 0 for integers)
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="errorMessage">Error message</param>
        public static IRuleBuilder<T, TProperty> NotEmpty<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            string errorMessage)
        {
            return ruleBuilder.SetValidator(new NotEmptyValidator(errorMessage));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is outside of the specifed range. The range is inclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minimumLength">Minimum length</param>
        /// <param name="maximumLength">Maximum length</param>
        public static IRuleBuilder<T, string> Length<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            int minimumLength,
            int maximumLength)
        {
            return ruleBuilder.Length(
                minimumLength,
                maximumLength,
                string.Format("Length must be between {0} and {1}", minimumLength, maximumLength));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is outside of the specifed range. The range is inclusive.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minimumLength">Minimum length</param>
        /// <param name="maximumLength">Maximum length</param>
        /// <param name="errorMessage">Error message</param>
        public static IRuleBuilder<T, string> Length<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            int minimumLength,
            int maximumLength,
            string errorMessage)
        {
            return ruleBuilder.SetValidator(
                new LengthValidator(minimumLength, maximumLength, errorMessage));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is not equal to the length specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="exactLength">Exact length</param>
        public static IRuleBuilder<T, string> Length<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            int exactLength)
        {
            return ruleBuilder.Length(
                exactLength,
                string.Format("Length must be exactly {0}", exactLength));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is not equal to the length specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="exactLength">Exact length</param>
        /// <param name="errorMessage">Error message</param>
        public static IRuleBuilder<T, string> Length<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            int exactLength,
            string errorMessage)
        {
            return ruleBuilder.SetValidator(
                new ExactLengthValidator(exactLength, errorMessage));
        }

        /// <summary>
        /// Defines a minimum length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is less than the length specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minimumLength">Minimum length</param>
        public static IRuleBuilder<T, string> MinimumLength<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            int minimumLength)
        {
            return ruleBuilder.MinimumLength(
                minimumLength,
                string.Format("Length must be greater than or equal to {0}", minimumLength));
        }

        /// <summary>
        /// Defines a minimum length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is less than the length specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minimumLength">Minimum length</param>
        /// <param name="errorMessage">Error message</param>
        public static IRuleBuilder<T, string> MinimumLength<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            int minimumLength,
            string errorMessage)
        {
            return ruleBuilder.SetValidator(
                new MinimumLengthValidator(minimumLength, errorMessage));
        }

        /// <summary>
        /// Defines a maximum length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is greater than the length specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="maximumLength">Maximum length</param>
        public static IRuleBuilder<T, string> MaximumLength<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            int maximumLength)
        {
            return ruleBuilder.MaximumLength(
                maximumLength,
                string.Format("Length must be less than or equal to {0}", maximumLength));
        }

        /// <summary>
        /// Defines a maximum length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is greater than the length specified.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="maximumLength">Maximum length</param>
        /// <param name="errorMessage">Error message</param>
        public static IRuleBuilder<T, string> MaximumLength<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            int maximumLength,
            string errorMessage)
        {
            return ruleBuilder.SetValidator(
                new MaximumLengthValidator(maximumLength, errorMessage));
        }

        /// <summary>
        /// Defines a regular expression validator on the current rule builder, but only for string properties.
        /// Validation will fail if the value returned by the lambda does not match the regular expression.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="expression">The regular expression to check the value against.</param>
        /// <param name="errorMessage">Error message</param>
        public static IRuleBuilder<T, string> Matches<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            string expression,
            string errorMessage)
        {
            return ruleBuilder.SetValidator(
                new RegularExpressionValidator(expression, errorMessage));
        }

        /// <summary>
        /// Defines a regular expression validator on the current rule builder, but only for string properties.
        /// Validation will fail if the value returned by the lambda does not match the regular expression.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="regex">The regular expression to use</param>
        /// <param name="errorMessage">Error message</param>
        public static IRuleBuilder<T, string> Matches<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            Regex regex,
            string errorMessage)
        {
            return ruleBuilder.SetValidator(
                new RegularExpressionValidator(regex, errorMessage));
        }

        /// <summary>
        /// Defines a regular expression validator on the current rule builder, but only for string properties.
        /// Validation will fail if the value returned by the lambda does not match the regular expression.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="expression">The regular expression to check the value against.</param>
        /// <param name="options">Regex options</param>
        /// <param name="errorMessage">Error message</param>
        public static IRuleBuilder<T, string> Matches<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            string expression,
            RegexOptions options,
            string errorMessage)
        {
            return ruleBuilder.SetValidator(
                new RegularExpressionValidator(expression, options, errorMessage));
        }

        /// <summary>
        /// Defines a 'greater than' validator on the current rule builder. 
        /// The validation will succeed if the property value is greater than the specified value.
        /// The validation will fail if the property value is less than or equal to the specified value.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompare">The value being compared</param>
        public static IRuleBuilder<T, TProperty> GreaterThan<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            TProperty valueToCompare)
            where TProperty : IComparable<TProperty>, IComparable
        {
            return ruleBuilder.GreaterThan(
                valueToCompare,
                string.Format("Must be greater than {0}", valueToCompare));
        }

        /// <summary>
        /// Defines a 'greater than' validator on the current rule builder. 
        /// The validation will succeed if the property value is greater than the specified value.
        /// The validation will fail if the property value is less than or equal to the specified value.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompare">The value being compared</param>
        /// <param name="errorMessage">Error message</param>
        public static IRuleBuilder<T, TProperty> GreaterThan<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            TProperty valueToCompare,
            string errorMessage)
            where TProperty : IComparable<TProperty>, IComparable
        {
            return ruleBuilder.SetValidator(
                new GreaterThanValidator(valueToCompare, errorMessage));
        }

        /// <summary>
        /// Defines a 'greater than or equal' validator on the current rule builder. 
        /// The validation will succeed if the property value is greater than or equal the specified value.
        /// The validation will fail if the property value is less than the specified value.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompare">The value being compared</param>
        public static IRuleBuilder<T, TProperty> GreaterThanOrEqual<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            TProperty valueToCompare)
            where TProperty : IComparable<TProperty>, IComparable
        {
            return ruleBuilder.GreaterThanOrEqual(
                valueToCompare,
                string.Format("Must be greater than or equal to {0}", valueToCompare));
        }

        /// <summary>
        /// Defines a 'greater than or equal' validator on the current rule builder. 
        /// The validation will succeed if the property value is greater than or equal the specified value.
        /// The validation will fail if the property value is less than the specified value.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompare">The value being compared</param>
        /// <param name="errorMessage">Error message</param>
        public static IRuleBuilder<T, TProperty> GreaterThanOrEqual<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            TProperty valueToCompare,
            string errorMessage)
            where TProperty : IComparable<TProperty>, IComparable
        {
            return ruleBuilder.SetValidator(
                new GreaterThanOrEqualValidator(valueToCompare, errorMessage));
        }

        /// <summary>
        /// Defines a 'less than' validator on the current rule builder. 
        /// The validation will succeed if the property value is less than the specified value.
        /// The validation will fail if the property value is greater than or equal to the specified value.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompare">The value being compared</param>
        public static IRuleBuilder<T, TProperty> LessThan<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            TProperty valueToCompare)
            where TProperty : IComparable<TProperty>, IComparable
        {
            return ruleBuilder.LessThan(
                valueToCompare,
                string.Format("Must be less than {0}", valueToCompare));
        }

        /// <summary>
        /// Defines a 'less than' validator on the current rule builder. 
        /// The validation will succeed if the property value is less than the specified value.
        /// The validation will fail if the property value is greater than or equal to the specified value.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompare">The value being compared</param>
        /// <param name="errorMessage">Error message</param>
        public static IRuleBuilder<T, TProperty> LessThan<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            TProperty valueToCompare,
            string errorMessage)
            where TProperty : IComparable<TProperty>, IComparable
        {
            return ruleBuilder.SetValidator(
                new LessThanValidator(valueToCompare, errorMessage));
        }

        /// <summary>
        /// Defines a 'less than or equal' validator on the current rule builder. 
        /// The validation will succeed if the property value is less than or equal to the specified value.
        /// The validation will fail if the property value is greater than the specified value.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompare">The value being compared</param>
        public static IRuleBuilder<T, TProperty> LessThanOrEqual<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            TProperty valueToCompare)
            where TProperty : IComparable<TProperty>, IComparable
        {
            return ruleBuilder.LessThanOrEqual(
                valueToCompare,
                string.Format("Must be less than or equal to {0}", valueToCompare));
        }

        /// <summary>
        /// Defines a 'less than or equal' validator on the current rule builder. 
        /// The validation will succeed if the property value is less than or equal to the specified value.
        /// The validation will fail if the property value is greater than the specified value.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompare">The value being compared</param>
        /// <param name="errorMessage">Error message</param>
        public static IRuleBuilder<T, TProperty> LessThanOrEqual<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            TProperty valueToCompare,
            string errorMessage)
            where TProperty : IComparable<TProperty>, IComparable
        {
            return ruleBuilder.SetValidator(
                new LessThanOrEqualValidator(valueToCompare, errorMessage));
        }

        /// <summary>
        /// Defines an 'equals' validator on the current rule builder. 
        /// Validation will fail if the specified value is not equal to the value of the property.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="toCompare">The value to compare</param>
        /// <param name="comparer">Equality Comparer to use</param>
        public static IRuleBuilder<T, TProperty> Equal<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            TProperty toCompare,
            IEqualityComparer comparer = null)
        {
            return ruleBuilder.Equal(
                toCompare,
                string.Format("Must be equal to {0}", toCompare), comparer);
        }

        /// <summary>
        /// Defines an 'equals' validator on the current rule builder. 
        /// Validation will fail if the specified value is not equal to the value of the property.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="toCompare">The value to compare</param>
        /// <param name="errorMessage">Error message</param>
        /// <param name="comparer">Equality Comparer to use</param>
        public static IRuleBuilder<T, TProperty> Equal<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            TProperty toCompare,
            string errorMessage,
            IEqualityComparer comparer = null)
        {
            return ruleBuilder.SetValidator(
                new EqualValidator(toCompare, comparer, errorMessage));
        }

        /// <summary>
        /// Defines an 'predicate' validator on the current rule builder. 
        /// Validation will fail if the value of the property does not fulfill the predicate
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <typeparam name="TProperty">Type of property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="predicate">The predicate</param>
        /// <param name="errorMessage">Error message</param>
        public static IRuleBuilder<T, TProperty> Must<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            Func<TProperty, Task<bool>> predicate,
            string errorMessage)
        {
            return ruleBuilder.SetValidator(
                new PredicateValidator<TProperty>(predicate, errorMessage));
        }

        /// <summary>
        /// Defines a condition on whther the current rule must be run
        /// </summary>
        /// <typeparam name="T">Type of the object being validated</typeparam>
        /// <typeparam name="TProperty">Type of the property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which this condition must be applied</param>
        /// <param name="predicate">Predicate</param>
        public static void When<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> ruleBuilder,
            Func<T, bool> predicate)
        {
            ruleBuilder.Configure(predicate);
        }

        /// <summary>
        /// Defines a negative condition on whether the current rule must be run
        /// </summary>
        /// <typeparam name="T">Type of the object being validated</typeparam>
        /// <typeparam name="TProperty">Type of the property being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which this condition must be applied</param>
        /// <param name="predicate">Negative condition</param>
        public static void Unless<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> ruleBuilder,
            Func<T, bool> predicate)
        {
            ruleBuilder.When(x => !predicate(x));
        }
    }
}
