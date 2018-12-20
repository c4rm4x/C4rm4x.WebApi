using C4rm4x.Tools.Utilities;
using System.IO;
using System.Web;

namespace C4rm4x.WebApi.Security.Acl
{
    internal static class HttpContextExtensions
    {
        public static byte[] GetBodyAsByteArray(
            this HttpContextBase context)
        {
            context.NotNull(nameof(context));

            using (var stream = new MemoryStream())
            {
                context.Request.InputStream.Seek(0, SeekOrigin.Begin);
                context.Request.InputStream.CopyTo(stream);

                return stream.ToArray();
            }
        }
    }
}
