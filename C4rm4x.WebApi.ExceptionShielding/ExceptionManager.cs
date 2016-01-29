#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.ExceptionShielding;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding
{
    /// <summary>
    /// Non-static entry point to the exception handling functionality.
    /// </summary>
    /// <remarks>
    /// Instances of ExceptionManager can be used to replace references to the static 
    /// ExceptionPolicy facade
    /// </remarks>
    public class ExceptionManager : IExceptionManager
    {
        private readonly IDictionary<string, IExceptionPolicyDefinition>
            _exceptionPolicies;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exceptionPolicies">All the exception policies that will be handled by this ExceptionManager</param>
        public ExceptionManager(
            params IExceptionPolicyDefinition[] exceptionPolicies)
            : this(exceptionPolicies.ToDictionary(e => e.PolicyName))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exceptionPolicies">All the exception policies that will be handled by this ExceptionManager</param>
        public ExceptionManager(
            IEnumerable<IExceptionPolicyDefinition> exceptionPolicies)
            : this(exceptionPolicies.ToDictionary(e => e.PolicyName))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exceptionPolicies">All the exception policies that will be handled by this ExceptionManager as a dictionary where the key is the policy name</param>
        public ExceptionManager(
            IDictionary<string, IExceptionPolicyDefinition> exceptionPolicies)
        {
            exceptionPolicies.NotNull(nameof(exceptionPolicies));

            _exceptionPolicies = exceptionPolicies;
        }

        /// <summary>
        /// Handles the specified exception object according to the rules configured for policyName
        /// </summary>
        /// <param name="exceptionToHandle">The exception to handle</param>
        /// <param name="policyName">The name of the policy to handle</param>        
        /// <returns>True if rethrowing an exception is recommended; otherwise, false</returns>
        /// <example>
        /// The following code shows the usage of the 
        /// exception handling framework.
        /// <code>
        ///    try
        ///    {
        ///        DoWork();
        ///    }
        ///    catch (Exception e)
        ///    {
        ///        if (exceptionManager.HandleException(e, name)) throw;
        ///    }
        /// </code>
        /// </example>
        public bool HandleException(
            Exception exceptionToHandle,
            string policyName = "default")
        {
            exceptionToHandle.NotNull(nameof(exceptionToHandle));
            policyName.NotNullOrEmpty(nameof(policyName));

            IExceptionPolicyDefinition exceptionPolicy;

            if (!_exceptionPolicies.TryGetValue(policyName, out exceptionPolicy))
                throw new InvalidOperationException("Exception policy not found");

            return exceptionPolicy.HandleException(exceptionToHandle);
        }
    }
}
