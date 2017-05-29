#region Using

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Validation
{
    /// <summary>
    /// Service that validates an specific object
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Validates an object using default ruleSet
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <returns>List with all validation error. Empty list if no validation error is found</returns>
        Task<List<ValidationError>> ValidateAsync(object objectToValidate);

        /// <summary>
        /// Validates an object for a given ruleSet
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <param name="ruleSetName">The name of the ruleset</param>
        /// <returns>List with all validation error. Empty list if no validation error is found</returns>
        Task<List<ValidationError>> ValidateAsync(
            object objectToValidate, 
            string ruleSetName);

        /// <summary>
        /// Checks to see whether the validator can validate objects of the specified type
        /// </summary>
        /// <returns>True if validator can validate object of the specified type. False otherwise</returns>
        bool CanValidateInstancesOf(Type type);
    }

    /// <summary>
    /// Generic version of the service that validates an object of the specified type
    /// </summary>
    /// <typeparam name="T">Type of the object to validate</typeparam>
    public interface IValidator<T> : IValidator
    {
        /// <summary>
        /// Validates an instance of type T using default ruleSet
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <returns>List with all validation error. Empty list if no validation error is found</returns>
        Task<List<ValidationError>> ValidateAsync(T objectToValidate);

        /// <summary>
        /// Validates an instance of type T for a given ruleSet
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <param name="ruleSetName">The name of the ruleset</param>
        /// <returns>List with all validation error. Empty list if no validation error is found</returns>
        Task<List<ValidationError>> ValidateAsync(
            T objectToValidate, 
            string ruleSetName);
    }
}
