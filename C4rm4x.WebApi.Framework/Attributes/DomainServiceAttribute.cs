#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Framework
{
    /// <summary>
    /// Flags the underlying class as Domain Service
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DomainServiceAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the type of the interface (or abstract class) that implement
        /// </summary>
        public Type InterfaceType { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interfaceType">The type of the interface that implments</param>
        public DomainServiceAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType;
        }
    }
}
