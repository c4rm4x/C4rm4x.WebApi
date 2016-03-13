#region Using

using C4rm4x.Tools.Utilities;

#endregion

namespace C4rm4x.WebApi.Framework.Validation
{
    /// <summary>
    /// Abstraction of a validation error
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// The property that is not valid
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// The error
        /// </summary>
        public string ErrorDescription { get; private set; }

        /// <summary>
        /// The value of the property
        /// </summary>
        public object PropertyValue { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyName">The property is not valid</param>
        /// <param name="propertyValue">The property value</param>
        /// <param name="errorDescription">The error description</param>
        public ValidationError(
            string propertyName,
            object propertyValue,
            string errorDescription)
        {
            propertyName.NotNullOrEmpty(nameof(propertyName));
            errorDescription.NotNullOrEmpty(nameof(errorDescription));

            PropertyName = propertyName;
            PropertyValue = propertyValue;
            ErrorDescription = errorDescription;
        }
    }
}
