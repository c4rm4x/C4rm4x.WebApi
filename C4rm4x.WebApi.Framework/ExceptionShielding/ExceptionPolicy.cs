#region Using

using C4rm4x.Tools.Utilities;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.ExceptionShielding
{
    /// <summary>
    /// Represents a policy with exception types and exception handlers
    /// </summary>
    public class ExceptionPolicy
    {
        private static IExceptionManager ExceptionManager { get; set; }

        /// <summary>
        /// Sets the exception manager
        /// </summary>
        /// <param name="exceptionManager">Exception manager</param>
        public static void SetExceptionManager(
            IExceptionManager exceptionManager)
        {
            exceptionManager.NotNull(nameof(exceptionManager));

            ExceptionPolicy.ExceptionManager = exceptionManager;
        }

        /// <summary>
        /// Handles the specified exception object according to the rules configured
        /// for such policy.        
        /// </summary>
        /// <remarks>
        /// If a rethrow is recommended and exceptionToThrow is null,
        /// then the original exception should be rethrown; 
        /// otherwise, the new exception should be thrown.
        /// </remarks>
        /// <param name="exceptionToHandle">Exception to be handled</param>
        /// <param name="policyName">Policy name</param>
        /// <param name="exceptionToThrow">Exception to be thrown in case rethrow is recommended</param>
        /// <returns>True when rethrowing an exception is recommended based on rules</returns>
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
        ///        Exception exceptionToThrow;
        ///        
        ///        if (ExceptionPolicy.HandleException(e, name, out exceptionToThrow))
        ///        {
        ///             if(exceptionToThrow == null)
        ///                 throw;
        ///             else
        ///                 throw exceptionToThrow;
        ///        }
        ///    }
        /// </code>
        /// </example>
        public static bool HandleException(
            Exception exceptionToHandle,
            string policyName,
            out Exception exceptionToThrow)
        {
            exceptionToThrow = null;

            try
            {
                return HandleException(exceptionToHandle, policyName);
            }
            catch (Exception exception)
            {
                exceptionToThrow = exception;

                return true;
            }
        }

        private static bool HandleException(
            Exception exceptionToHandle,
            string policyName = "default")
        {
            exceptionToHandle.NotNull(nameof(exceptionToHandle));
            policyName.NotNullOrEmpty(nameof(policyName));

            return ExceptionManager
                .HandleException(exceptionToHandle, policyName);
        }
    }
}
