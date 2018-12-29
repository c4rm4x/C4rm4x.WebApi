using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace C4rm4x.WebApi.Events.EF
{
    internal class ApiEventDataSerializerContractResolver : DefaultContractResolver
    {
        private static IEnumerable<string> Properties = new[]
        {
            nameof(ApiEventData.Id),
            nameof(ApiEventData.Version),
            nameof(ApiEventData.TimeStamp)
        };

        private readonly IEnumerable<Type> TypesToIgnore;

        public ApiEventDataSerializerContractResolver(
            IEnumerable<Type> typesToIgnore)
        {
            typesToIgnore.NotNull(nameof(typesToIgnore));

            TypesToIgnore = typesToIgnore;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (IsApiEventDataProperty(property.PropertyName) ||
                IsTypeToIgnore(property.PropertyType))
            {
                property.ShouldSerialize = i => false;
                property.Ignored = true;
            }

            return property;
        }

        private static bool IsApiEventDataProperty(string propertyName) => Properties.Contains(propertyName);

        private bool IsTypeToIgnore(Type type) => TypesToIgnore
            .FirstOrDefault(_ => _.IsAssignableFrom(type))
            .IsNotNull();
    }
}