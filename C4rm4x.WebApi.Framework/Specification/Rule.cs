#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Validation;

#endregion

namespace C4rm4x.WebApi.Framework.Specification
{
    #region Interface

    /// <summary>
    /// Business rule descriptor
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// The error
        /// </summary>
        ValidationError Error { get; }
    }

    #endregion

    /// <summary>
    /// Base class that implements interface IRule
    /// </summary>
    public class Rule : IRule
    {
        /// <summary>
        /// Gets the code
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Gets the description
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="description">The description</param>
        public Rule(
            string code,
            string description)
        {
            code.NotNullOrEmpty(nameof(code));
            description.NotNullOrEmpty(nameof(description));

            Code = code;
            Description = description;
        }

        /// <summary>
        /// The error
        /// </summary>
        public ValidationError Error
        {
            get { return new ValidationError(Code, null, Description); }
        }
    }
}
