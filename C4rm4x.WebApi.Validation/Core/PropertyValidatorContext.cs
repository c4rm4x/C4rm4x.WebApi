#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Validation.Core
{
    /// <summary>
    /// Validation context for a member of the object to validate
    /// </summary>
    public class PropertyValidatorContext
    {
        private bool _hasPropertyValueSet;
        private object _propertyValue;

        /// <summary>
        /// Gets the parent context (if any)
        /// </summary>
        public ValidationContext ParentContext { get; private set; }

        /// <summary>
        /// Gets the property rule to apply
        /// </summary>
        public PropertyRule Rule { get; private set; }

        /// <summary>
        /// Gets the property name
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Gets the instance to validate
        /// </summary>
        public object Instance
        {
            get { return ParentContext.InstanceToValidate; }
        }

        /// <summary>
        /// Gets or sets property value
        /// </summary>
        public object PropertyValue
        {
            get
            {
                // Lazily load the property value to allow the delegating validator 
                // to cancel validation before value is obtained
                if (!_hasPropertyValueSet)
                    this.PropertyValue = Rule.PropertyFunc(Instance);

                return _propertyValue;
            }
            set
            {
                _propertyValue = value;
                _hasPropertyValueSet = true;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parentContext">The parent context</param>
        /// <param name="rule">Property rule to apply</param>
        /// <param name="propertyName">Property name</param>
        public PropertyValidatorContext(
            ValidationContext parentContext,
            PropertyRule rule,
            string propertyName)
        {
            ParentContext = parentContext;
            Rule = rule;
            PropertyName = propertyName;
        }
    }
}
