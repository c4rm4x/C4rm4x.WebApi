#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding
{
    #region Interface

    /// <summary>
    /// Represents a policy for handling exceptions
    /// </summary>
    public interface IExceptionPolicyDefinition
    {
        /// <summary>
        /// Policy name for this definition
        /// </summary>
        string PolicyName { get; }

        /// <summary>
        /// Checks if there is a policy entry that matches
        /// the type of the specified exception,
        /// and if so, invokes the handlers associated with that entry
        /// </summary>
        /// <param name="exceptionToHandle">The exception to handle</param>
        /// <returns>True when rethrowing an exception is recommended</returns>
        bool HandleException(Exception exceptionToHandle);
    }

    #endregion

    /// <summary>
    /// Represents a policy for handling exceptions
    /// </summary>
    public class ExceptionPolicyDefinition : IExceptionPolicyDefinition
    {
        private readonly IDictionary<Type, IExceptionPolicyEntry>
            _policyEntries;

        /// <summary>
        /// Gets the policy name for this definition
        /// </summary>
        public string PolicyName { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="policyName">The policy name for this definition</param>
        /// <param name="policyEntries">A set of IExceptionPolicyEntry instances</param>
        public ExceptionPolicyDefinition(
            string policyName,
            IEnumerable<IExceptionPolicyEntry> policyEntries)
            : this(policyName, policyEntries.ToDictionary(e => e.ExceptionType))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="policyName">The policy name for this definition</param>
        /// <param name="policyEntries">A set of IExceptionPolicyEntry instances as a dictionary where the key is the type of exception to handle</param>
        public ExceptionPolicyDefinition(
            string policyName,
            IDictionary<Type, IExceptionPolicyEntry> policyEntries)
        {
            policyEntries.NotNull(nameof(policyEntries));
            policyName.NotNullOrEmpty(nameof(policyName));

            PolicyName = policyName;
            _policyEntries = policyEntries;
        }

        /// <summary>
        /// Checks if there is a policy entry that matches
        /// the type of the specified exception,
        /// and if so, invokes the handlers associated with that entry
        /// </summary>
        /// <param name="exceptionToHandle">The exception to handle</param>
        /// <returns>True when rethrowing an exception is recommended</returns>
        public bool HandleException(Exception exceptionToHandle)
        {
            exceptionToHandle.NotNull(nameof(exceptionToHandle));

            var entry = GetPolicyEntry(exceptionToHandle);

            return entry.IsNull()
                ? true
                : entry.Handle(exceptionToHandle);
        }

        private IExceptionPolicyEntry GetPolicyEntry(Exception ex)
        {
            return this.FindExceptionPolicyEntry(ex.GetType());
        }

        private IExceptionPolicyEntry FindExceptionPolicyEntry(
            Type exceptionType)
        {
            IExceptionPolicyEntry entry = null;

            while (exceptionType != typeof(Object))
            {
                entry = GetPolicyEntry(exceptionType);

                if (entry != null) break;

                exceptionType = exceptionType.BaseType;
            }

            return entry;
        }

        private IExceptionPolicyEntry GetPolicyEntry(Type exceptionType)
        {
            return _policyEntries.ContainsKey(exceptionType)
                ? _policyEntries[exceptionType]
                : null;
        }
    }
}
