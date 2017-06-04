#region Using

using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation.Core
{
    /// <summary>
    /// Defines a rule associated with a property.
    /// </summary>
    public class PropertyRule : IValidationRule
    {
        private readonly List<IPropertyValidator> _validators =
            new List<IPropertyValidator>();

        private static Func<object, bool> True = (x) => true;

        /// <summary>
        /// Gets the list of all validators for this property
        /// </summary>
        public IEnumerable<IPropertyValidator> Validators
        {
            get { return _validators; }
        }

        /// <summary>
        /// Rule set that this rule belongs to (if specified)
        /// </summary>
        public string RuleSet { get; set; }

        /// <summary>
        /// Property associated with this rule
        /// </summary>
        public MemberInfo Member { get; private set; }

        /// <summary>
        /// Function that can be invoked to retrieve the value of the property
        /// </summary>
        public Func<object, object> PropertyFunc { get; private set; }

        /// <summary>
        /// Expression that was used to create the rule.
        /// </summary>
        public LambdaExpression Expression { get; private set; }

        /// <summary>
        /// Type of the property being validated
        /// </summary>
        public Type TypeToValidate { get; private set; }

        /// <summary>
        /// Propety name associated with this rule
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Execution this rule only if 
        /// </summary>
        public Func<object, bool> When { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="member">Property</param>
        /// <param name="propertyFunc">Function to get the property value</param>
        /// <param name="expression">Lambda expression used to create the rule</param>
        /// <param name="typeToValidate">Type to validate</param>
        /// <param name="containerType">Container type that owns the property</param>
        /// <param name="when">Function to evaluate whether or not this rule must be executed</param>
        public PropertyRule(
            MemberInfo member,
            Func<object, object> propertyFunc,
            LambdaExpression expression,
            Type typeToValidate,
            Type containerType,
            Func<object, bool> when = null)
        {
            Member = member;
            PropertyFunc = propertyFunc;
            Expression = expression;
            TypeToValidate = typeToValidate;
            PropertyName = GetPropertyName(containerType, member, expression);
            When = when ?? True;
        }

        private static string GetPropertyName(
            Type containerType,
            MemberInfo member,
            LambdaExpression expression)
        {
            if (expression != null)
                return PropertyChain.FromExpression(expression).ToString();

            if (member != null)
                return member.Name;

            return null;
        }

        /// <summary>
        /// Performs validation using a validation context and returns a collection of 
        /// Validation failures
        /// </summary>
        /// <param name="context">Validation Context</param>
        /// <returns>A collection of validation failures</returns>
        public async Task<IEnumerable<ValidationError>> ValidateAsync(ValidationContext context)
        {
            if (string.IsNullOrEmpty(PropertyName))
                throw new InvalidOperationException(
                    string.Format(
                        "Property name could not be automatically determined for expression {0}",
                        Expression));

            var errors = new List<ValidationError>();

            // Ensure that this rule is allowed to run. 
            if (context.ValidatorSelector.CanExecute(this, PropertyName, context) &&
                When(context.InstanceToValidate))
            {
                // Invoke each validator and collect its results.
                var tasks = _validators
                    .Select(validator => InvokePropertyValidatorAsync(context, validator, PropertyName));
                var results = await Task.WhenAll(tasks);

                errors.AddRange(results.SelectMany(r => r));
            }

            return errors;
        }

        /// <summary>
        /// Invokes a property validator using the specified validation context
        /// </summary>
        protected virtual Task<IEnumerable<ValidationError>> InvokePropertyValidatorAsync(
            ValidationContext context,
            IPropertyValidator validator,
            string propertyName)
        {
            return validator.ValidateAsync(
                new PropertyValidatorContext(context, this, propertyName));
        }

        /// <summary>
        /// Creates a new property rule from a lambda expression
        /// </summary>
        public static PropertyRule Create<T, TProperty>(
            Expression<Func<T, TProperty>> expression)
        {
            var member = expression.GetMember();
            var compiled = expression.Compile();

            return new PropertyRule(member, x => compiled((T)x), expression, typeof(TProperty), typeof(T));
        }

        /// <summary>
        /// Adds a validator to the rule
        /// </summary>
        /// <param name="validator">The validator</param>
        public void AddValidator(IPropertyValidator validator)
        {
            _validators.Add(validator);
        }

        /// <summary>
        /// Sets the condition on whether this rule must be run
        /// </summary>
        /// <param name="condition">The condition</param>
        public void ApplyCondition(Func<object, bool> condition)
        {
            When = condition;
        }
    }
}
