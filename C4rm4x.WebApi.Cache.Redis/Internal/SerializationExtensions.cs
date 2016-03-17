#region Using

using C4rm4x.Tools.Utilities;
using Newtonsoft.Json;
using System;

#endregion

namespace C4rm4x.WebApi.Cache.Redis
{
    internal static class SerializationExtensions
    {
        public static T DeserializeAs<T>(this string value)
        {
            if (value.IsNullOrEmpty()) return default(T);

            var destType = typeof(T);

            try
            {
                if (destType.IsValueType || destType == typeof(string))
                    return (T)Convert.ChangeType(value, destType);
            }
            catch (FormatException) // Convert format exception into invalid cast
            {
                throw new InvalidCastException();
            }

            return JsonConvert.DeserializeObject<T>(value);
        }

        public static string SerializeAsString(this object obj)
        {
            obj.NotNull(nameof(obj));

            var sourceType = obj.GetType();

            return (sourceType.IsValueType || sourceType == typeof(string))
                ? obj.ToString()
                : JsonConvert.SerializeObject(obj);
        }
    }
}
