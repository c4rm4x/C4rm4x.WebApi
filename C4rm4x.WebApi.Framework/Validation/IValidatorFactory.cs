#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Framework.Validation
{
    /// <summary>
    /// Service responsible to retrieve the instance that implements
    /// IValidator for the specified type
    /// </summary>
    public interface IValidatorFactory
    {
        /// <summary>
        /// Gets the validator for the specified type.
        /// </summary>
        IValidator<T> GetValidator<T>();

        /// <summary>
        /// Gets the validator for the specified type.
        /// </summary>
        IValidator GetValidator(Type type);
    }
}
