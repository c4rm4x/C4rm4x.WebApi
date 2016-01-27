#region Using

using System;
using System.Collections.Generic;
using C4rm4x.Tools.Utilities;

#endregion

namespace C4rm4x.WebApi.Framework
{
    /// <summary>
    /// Flags the underlying class as Request Handler
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RequestHandlerAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the types of the requests to handle
        /// </summary>
        public IEnumerable<Type> TypesToHandle { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typesToHandle">Types of the requests to handle</param>
        public RequestHandlerAttribute(params Type[] typesToHandle)
        {
            foreach (var typeToHandle in typesToHandle)
                typeToHandle.Is<ApiRequest>();

            TypesToHandle = typesToHandle;
        }
    }
}
