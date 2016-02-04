#region Using

using C4rm4x.Tools.Utilities;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;

#endregion

namespace C4rm4x.WebApi.Framework.RequestHandling.Results
{
    internal static class HttpResponseMessageUtils
    {
        internal static HttpResponseMessage Create<TContent>(
            HttpStatusCode statusCode,
            TContent content,
            MediaTypeFormatter formatter = null,
            string mediaType = "application/json",
            bool goingCamelCase = true)
            where TContent : class
        {
            content.NotNull(nameof(content));

            var response = new HttpResponseMessage();

            try
            {
                response.StatusCode = statusCode;
                response.Content = new ObjectContent<TContent>(
                    content, formatter ?? GetJsonMediaTypeFormatter(goingCamelCase), mediaType);
            }
            catch
            {
                response.Dispose();
                throw;
            }

            return response;
        }

        private static MediaTypeFormatter GetJsonMediaTypeFormatter(
            bool goingCamelCase)
        {
            var formatter = new JsonMediaTypeFormatter();

            formatter.SerializerSettings.ContractResolver = 
                new CamelCasePropertyNamesContractResolver();

            return formatter;
        }
    }
}
