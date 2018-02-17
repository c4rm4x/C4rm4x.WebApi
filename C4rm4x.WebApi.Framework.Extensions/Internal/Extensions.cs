#region Using

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Framework
{
    internal static class Extensions
    {
        public static IEnumerable<KnownTypeAttribute> GetKnownTypeAttributes(this Type type)
        {
            return Attribute.GetCustomAttributes(type).OfType<KnownTypeAttribute>();
        }

        public static IEnumerable<string> GetKeys(this JObject obj)
        {
            foreach (var kp in obj) yield return kp.Key;
        }
    }
}
