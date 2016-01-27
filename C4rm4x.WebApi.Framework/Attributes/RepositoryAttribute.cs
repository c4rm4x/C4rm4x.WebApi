#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Framework
{
    /// <summary>
    /// Flags the underlying class as Repository
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RepositoryAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the type of the entity to handle access data of
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityType">Type of the entity to handle access data of</param>
        public RepositoryAttribute(Type entityType)
        {
            EntityType = entityType;
        }
    }
}
