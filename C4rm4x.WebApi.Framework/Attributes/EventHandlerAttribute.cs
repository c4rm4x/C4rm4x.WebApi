#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Framework
{
    /// <summary>
    /// Flags the underlying class as Event Handler
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EventHandlerAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the types of the events to handle
        /// </summary>
        public IEnumerable<Type> TypesToHandle { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typesToHandle">Types of the events to handle</param>
        public EventHandlerAttribute(params Type[] typesToHandle)
        {
            foreach (var typeToHandle in typesToHandle)
                typeToHandle.Is<ApiEventData>();

            TypesToHandle = typesToHandle;
        }
    }
}
