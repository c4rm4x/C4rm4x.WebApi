#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Framework
{
    /// <summary>
    /// Flags the underlying class as Specification
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SpecificationAttribute : Attribute
    {
    }
}
