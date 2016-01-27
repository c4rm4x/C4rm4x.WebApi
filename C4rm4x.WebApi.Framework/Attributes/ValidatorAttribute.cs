#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Framework.Attributes
{
    /// <summary>
    /// Flags the underlying class as Validator
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidatorAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the type of the object to validate
        /// </summary>
        public Type TypeToValidate { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typeToValidate">Type of the object to validate</param>
        public ValidatorAttribute(Type typeToValidate)
        {
            TypeToValidate = typeToValidate;
        }
    }
}
