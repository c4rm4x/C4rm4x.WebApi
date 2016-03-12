#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Validation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace C4rm4x.WebApi.Validation
{
    /// <summary>
    /// Base implementation of IValidator
    /// </summary>
    /// <typeparam name="T">Type of object to validate</typeparam>
    public abstract class AbstractValidator<T> : IValidator<T>
    {
        private readonly TrackingCollection<IValidationRule> _nestedValidators =
            new TrackingCollection<IValidationRule>();

        /// <summary>
        /// Validates an instance of type T using default ruleSet
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <returns>List with all validation error. Empty list if no validation error is found</returns>
        public List<ValidationError> Validate(T objectToValidate)
        {
            return Validate(objectToValidate, new DefaultValidatorSelector());
        }

        /// <summary>
        /// Validates an instance of type T for a given ruleSet
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <param name="ruleSetName">The name of the ruleset</param>
        /// <returns>List with all validation error. Empty list if no validation error is found</returns>
        public List<ValidationError> Validate(
            T objectToValidate,
            string ruleSetName)
        {
            ruleSetName.NotNullOrEmpty(nameof(ruleSetName));

            return Validate(objectToValidate, new RulesetValidatorSelector(ruleSetName));
        }

        private List<ValidationError> Validate(
            T objectToValidate,
            IValidatorSelector validatorSelector)
        {
            objectToValidate.NotNull(nameof(objectToValidate));

            var context = new ValidationContext<T>(objectToValidate, validatorSelector);

            return _nestedValidators.SelectMany(x => x.Validate(context)).ToList();
        }

        /// <summary>
        /// Validates an instance of type T using default ruleSet and throws an exception if validation fails
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <exception cref="ValidationException">If validation fails</exception>
        public void ThrowIf(T objectToValidate)
        {
            var validationErrors = Validate(objectToValidate);

            if (validationErrors.Any())
                throw new ValidationException(validationErrors);
        }

        /// <summary>
        /// Validates an instance of type T using a given ruleSet and throws an exception if validation fails
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <param name="ruleSetName">The name of the ruleset</param>
        /// <exception cref="ValidationException">If validation fails</exception>
        public void ThrowIf(
            T objectToValidate,
            string ruleSetName)
        {
            var validationErrors = Validate(objectToValidate, ruleSetName);

            if (validationErrors.Any())
                throw new ValidationException(validationErrors);
        }

        /// <summary>
        /// Validates an object using default ruleSet
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <returns>List with all validation error. Empty list if no validation error is found</returns>
        /// <exception cref="ArgumentException">If validator cannot validate an instance of given type</exception>
        public List<ValidationError> Validate(object objectToValidate)
        {
            objectToValidate.NotNull(nameof(objectToValidate));

            if (!this.CanBeValidated(objectToValidate))
                throw new ArgumentException(
                    string.Format("And object of type {0} cannot be validated against this validator {1}",
                    objectToValidate.GetType().FullName, this.GetType().FullName));

            return Validate((T)objectToValidate);
        }

        /// <summary>
        /// Validates an object for a given ruleSet
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <param name="ruleSetName">The name of the ruleset</param>
        /// <returns>List with all validation error. Empty list if no validation error is found</returns>
        /// <exception cref="ArgumentException">If validator cannot validate an instance of given type</exception>
        public List<ValidationError> Validate(
            object objectToValidate,
            string ruleSetName)
        {
            objectToValidate.NotNull(nameof(objectToValidate));
            ruleSetName.NotNullOrEmpty(nameof(ruleSetName));

            if (!this.CanBeValidated(objectToValidate))
                throw new ArgumentException(
                    string.Format("And object of type {0} cannot be validated against this validator {1}",
                    objectToValidate.GetType().FullName, this.GetType().FullName));

            return Validate((T)objectToValidate, ruleSetName);
        }

        /// <summary>
        /// Validates an object using default ruleSet and throws an exception if validation fails
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <exception cref="ValidationException">If validation fails</exception>
        /// <exception cref="ArgumentException">If validator cannot validate an instance of given type</exception>
        public void ThrowIf(object objectToValidate)
        {
            objectToValidate.NotNull(nameof(objectToValidate));

            if (!this.CanBeValidated(objectToValidate))
                throw new ArgumentException(
                    string.Format("And object of type {0} cannot be validated against this validator {1}",
                    objectToValidate.GetType().FullName, this.GetType().FullName));

            ThrowIf((T)objectToValidate);
        }

        /// <summary>
        /// Validates an object using a given ruleSet and throws an exception if validation fails
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <param name="ruleSetName">The name of the ruleset</param>
        /// <exception cref="ValidationException">If validation fails</exception>
        /// <exception cref="ArgumentException">If validator cannot validate an instance of given type</exception>
        public void ThrowIf(object objectToValidate, string ruleSetName)
        {
            objectToValidate.NotNull(nameof(objectToValidate));
            ruleSetName.NotNullOrEmpty(nameof(ruleSetName));

            if (!this.CanBeValidated(objectToValidate))
                throw new ArgumentException(
                    string.Format("And object of type {0} cannot be validated against this validator {1}",
                    objectToValidate.GetType().FullName, this.GetType().FullName));

            ThrowIf((T)objectToValidate, ruleSetName);
        }

        /// <summary>
        /// Checks to see whether the validator can validate objects of the specified type
        /// </summary>
        /// <returns>True if validator can validate object of the specified type. False otherwise</returns>
        public bool CanValidateInstancesOf(Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }

        private bool CanBeValidated(object objectToValidate)
        {
            return CanValidateInstancesOf(objectToValidate.GetType());
        }

        /// <summary>
        /// Adds a rule to the current validator.
        /// </summary>
        /// <param name="rule">Rule to add</param>
        public void AddRule(IValidationRule rule)
        {
            _nestedValidators.Add(rule);
        }

        /// <summary>
        /// Defines a validation rule for a specify property.
        /// </summary>
        /// <example>
        /// RuleFor(x => x.Surname)...
        /// </example>
        /// <typeparam name="TProperty">The type of property being validated</typeparam>
        /// <param name="expression">The expression representing the property to validate</param>
        /// <returns>an IRuleBuilder instance on which validators can be defined</returns>
        public IRuleBuilder<T, TProperty> RuleFor<TProperty>(
            Expression<Func<T, TProperty>> expression)
        {
            expression.NotNull(nameof(expression));

            var rule = PropertyRule.Create(expression);

            AddRule(rule);

            return new RuleBuilder<T, TProperty>(rule);
        }

        /// <summary>
        /// Defines a RuleSet that can be used to group together several validators
        /// </summary>
        /// <param name="ruleSetName">The name of the ruleset</param>
        /// <param name="action">Action that encapsulates the rules in the ruleset</param>
        public void RuleSet(
            string ruleSetName,
            Action action)
        {
            ruleSetName.NotNullOrEmpty(nameof(ruleSetName));
            action.NotNull(nameof(action));

            using (_nestedValidators.OnItemAdded(r => r.RuleSet = ruleSetName))
            {
                action();
            }
        }
    }
}
