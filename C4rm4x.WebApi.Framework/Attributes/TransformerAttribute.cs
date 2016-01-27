#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Framework
{
    /// <summary>
    /// Flags the underlying class as Transformer
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TransformerAttribute : Attribute
    {
    }
}
