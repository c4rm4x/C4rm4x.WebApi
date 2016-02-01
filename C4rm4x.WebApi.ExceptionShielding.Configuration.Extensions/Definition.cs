#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions
{
    /// <summary>
    /// Abstraction of ExceptionPolicyDefinition
    /// </summary>
    public class Definition
    {
        private readonly IDictionary<Type, Entry> _entries;

        /// <summary>
        /// Gets the policy name for this definition
        /// </summary>
        public string PolicyName { get; private set; }

        private Definition(string policyName)
        {
            policyName.NotNullOrEmpty(nameof(policyName));

            PolicyName = policyName;
            _entries = new Dictionary<Type, Entry>();
        }

        /// <summary>
        /// Creates a new instance of Definition
        /// </summary>
        /// <remarks>
        /// Entry point to start configuring a new definition
        /// </remarks>
        /// <param name="policyName">The policy name</param>
        /// <returns>A new instance of Definition</returns>
        public static Definition WithName(string policyName)
        {
            return new Definition(policyName);
        }

        /// <summary>
        /// Adds a new entry (abstraction of ExceptionPolicyEntry)
        /// within this definition
        /// </summary>
        /// <param name="entry">The entry</param>
        /// <returns>This definition</returns>
        public Definition WithExceptionPolicyEntry(
            Entry entry)
        {
            entry.NotNull(nameof(entry));

            if (_entries.ContainsKey(entry.ExceptionType))
                throw new ArgumentException(
                    "Exception type {0} has been already defined within this definition"
                    .AsFormat(entry.ExceptionType.Name));

            _entries.Add(entry.ExceptionType, entry);

            return this;
        }

        /// <summary>
        /// Gets the equivalent ExceptionPolicyDefinition for this one
        /// </summary>
        /// <returns>A new ExceptionPolicyDefinition instance based on this configuration</returns>
        public ExceptionPolicyDefinition GetExceptionPolicyDefinition()
        {
            return new ExceptionPolicyDefinition(
                PolicyName,
                _entries.Values.Select(
                    entry => entry.GetExceptionPolicyEntry()));
        }
    }
}
