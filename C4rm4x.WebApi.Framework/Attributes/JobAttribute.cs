#region Using

using C4rm4x.Tools.Utilities;
using System;

#endregion

namespace C4rm4x.WebApi.Framework
{
    /// <summary>
    /// Flags the underlying class as Job
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JobAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the type (or abstract class) that implements
        /// an scheduled tasks of type IProcessor
        /// </summary>
        public Type ProcessorType { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="processorType">The type that implments IProcessor</param>
        public JobAttribute(Type processorType)
        {
            processorType.Is<IProcessor>();

            ProcessorType = processorType;
        }
    }
}
