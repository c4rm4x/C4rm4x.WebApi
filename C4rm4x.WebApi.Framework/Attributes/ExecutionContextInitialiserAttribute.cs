#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Framework
{
    /// <summary>
    /// Flags the underlying class as Execution Context Initialiser
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExecutionContextInitialiserAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the types of the requests to initialise execution context for
        /// </summary>
        public IEnumerable<Type> TypesOfRequests { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typesOfRequests">Types of the requests to initialise execution context for</param>
        public ExecutionContextInitialiserAttribute(params Type[] typesOfRequests)
        {
            foreach (var typeOfRequest in typesOfRequests)
                typeOfRequest.Is<ApiEventData>();

            TypesOfRequests = typesOfRequests;
        }
    }
}
