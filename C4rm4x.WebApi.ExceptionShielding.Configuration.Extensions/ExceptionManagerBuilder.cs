#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration
{
    /// <summary>
    /// Configures ExceptionManager using a fluent interface
    /// </summary>
    public class ExceptionManagerBuilder
    {
        private readonly IDictionary<string, Definition>
            _definitions;

        private ExceptionManagerBuilder()
        {
            _definitions =
                new Dictionary<string, Definition>();
        }

        /// <summary>
        /// Creates a new instance of ExceptionManagerBuilder
        /// </summary>
        /// <remarks>
        /// This is the entry point to start configuring a new Exceptionmanager
        /// </remarks>
        /// <returns>A new instance of ExceptionManagerBuilder</returns>
        public static ExceptionManagerBuilder Configure()
        {
            return new ExceptionManagerBuilder();
        }

        /// <summary>
        /// Adds a new definition (ExceptionPolicyDefinition abstraction)
        /// within this builder
        /// </summary>
        /// <param name="definition">A new definition</param>
        /// <returns>This builder</returns>
        public ExceptionManagerBuilder WithExceptionPolicyDefinition(
            Definition definition)
        {
            definition.NotNull(nameof(definition));

            if (_definitions.ContainsKey(definition.PolicyName))
                throw new ArgumentException(
                    "PolicyName {0} has been already defined".AsFormat(definition.PolicyName));

            _definitions.Add(definition.PolicyName, definition);

            return this;
        }

        /// <summary>
        /// Creates a new instance of ExceptionManager with the configuration specified at this moment
        /// </summary>
        /// <returns>A new intance of ExceptionManager configured with the specified rules</returns>
        public ExceptionManager Build()
        {
            return new ExceptionManager(
                _definitions.Values.Select(
                    defintion => defintion.GetExceptionPolicyDefinition()));
        }
    }
}
