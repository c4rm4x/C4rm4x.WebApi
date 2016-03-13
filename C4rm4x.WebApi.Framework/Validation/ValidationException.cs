#region Using

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Framework.Validation
{
    /// <summary>
    /// Validation exceptions
    /// </summary>
    public class ValidationException : ApiException
    {
        private new const string Code = "SYS_001";

        /// <summary>
        /// Gets the collection of all validation errors
        /// </summary>
        public IEnumerable<ValidationError> ValidationErrors { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validationError">A validation error</param>
        public ValidationException(ValidationError validationError)
            : this(new[] { validationError })
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validationErrors">The collection of all validation errors</param>
        public ValidationException(IEnumerable<ValidationError> validationErrors)
            : this(validationErrors, null)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validationErrors">The collection of all validation errors</param>
        /// <param name="innerException">Inner exception</param>
        public ValidationException(
            IEnumerable<ValidationError> validationErrors,
            Exception innerException)
            : base(Code, GetMessage(validationErrors), innerException)
        {
            ValidationErrors = validationErrors;
        }

        private static string GetMessage(
            IEnumerable<ValidationError> validationErrors)
        {
            return string.Join(",", validationErrors.Select(v =>
                string.Format("{0}: {1}", v.PropertyName, v.ErrorDescription)));
        }
    }
}
