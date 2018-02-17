#region Using

using C4rm4x.Tools.Utilities;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework
{
    public static class HttpConfigurationExtensions
    {
        public static void AddKnonwTypeConverter(
            this HttpConfiguration configuration)
        {
            configuration.NotNull(nameof(configuration));

            configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
                new KnownTypeConverter());
        }
    }
}
