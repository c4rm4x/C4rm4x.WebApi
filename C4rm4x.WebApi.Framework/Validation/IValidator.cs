#region Using

using System;
using System.Collections.Generic;

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
        /// <exception cref="ValidationException">If validator cannot validate an instance of given type</exception>
        List<ValidationError> Validate(object objectToValidate);

        /// <summary>
        /// Validates an object for a given ruleSet
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <param name="ruleSetName">The name of the ruleset</param>
        /// <returns>List with all validation error. Empty list if no validation error is found</returns>
        /// <exception cref="ValidationException">If validator cannot validate an instance of given type</exception>
        List<ValidationError> Validate(object objectToValidate, string ruleSetName);

        /// <summary>
        /// Validates an object using default ruleSet and throws an exception if validation fails
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <exception cref="ValidationException">If validation fails</exception>
        /// <exception cref="ValidationException">If validator cannot validate an instance of given type</exception>
        void ThrowIf(object objectToValidate);

        /// <summary>
        /// Validates an object using a given ruleSet and throws an exception if validation fails
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <param name="ruleSetName">The name of the ruleset</param>
        /// <exception cref="ValidationException">If validation fails</exception>
        /// <exception cref="ValidationException">If validator cannot validate an instance of given type</exception>
        void ThrowIf(object objectToValidate, string ruleSetName);

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
        List<ValidationError> Validate(T objectToValidate);

        /// <summary>
        /// Validates an instance of type T for a given ruleSet
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <param name="ruleSetName">The name of the ruleset</param>
        /// <returns>List with all validation error. Empty list if no validation error is found</returns>
        List<ValidationError> Validate(T objectToValidate, string ruleSetName);

        /// <summary>
        /// Validates an instance of type T using default ruleSet and throws an exception if validation fails
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <exception cref="ValidationException">If validation fails</exception>
        void ThrowIf(T objectToValidate);

        /// <summary>
        /// Validates an instance of type T using a given ruleSet and throws an exception if validation fails
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <param name="ruleSetName">The name of the ruleset</param>
        /// <exception cref="ValidationException">If validation fails</exception>
        void ThrowIf(T objectToValidate, string ruleSetName);
    }
}
