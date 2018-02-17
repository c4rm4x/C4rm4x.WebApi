#region Using

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Framework
{
    public class KnownTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetKnownTypeAttributes().Any();
        }

        public override bool CanWrite => false;

        public override object ReadJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            foreach (var attr in objectType.GetKnownTypeAttributes())
            {
                var target = Activator.CreateInstance(attr.Type);

                JObject jTest;

                using (var writer = new StringWriter())
                {
                    using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        serializer.Serialize(jsonWriter, target);
                        jTest = JObject.Parse(writer.ToString());
                    }
                }

                var jO = jObject.GetKeys().ToList();
                var jT = jTest.GetKeys().ToList();

                if (jO.Count == jT.Count &&
                    jO.Intersect(jT, StringComparer.InvariantCultureIgnoreCase).Count() == jO.Count)
                {
                    serializer.Populate(jObject.CreateReader(), target);
                    return target;
                }
            }

            return null;
        }

        public override void WriteJson(
            JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
