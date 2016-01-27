#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Framework.ExceptionShielding
{
    /// <summary>
    /// Non-static entry point to the exception handling functionality
    /// </summary>
    /// <remarks>
    /// Instances of ExceptionManager can be used to replace references to the 
    /// static ExceptionPolicy facade
    /// </remarks>
    public interface IExceptionManager
    {
        /// <summary>
        /// Handles an exception based on rules configured for such policy
        /// </summary>
        /// <param name="exceptionToHandle">Exception to handle</param>
        /// <param name="policyName">Policy name</param>
        /// <returns>True when rethrowning an exception is recommended based on rules</returns>
        bool HandleException(
            Exception exceptionToHandle,
            string policyName = "default");
    }
}
