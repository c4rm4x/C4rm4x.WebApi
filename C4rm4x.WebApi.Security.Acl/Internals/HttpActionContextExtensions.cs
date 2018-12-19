using C4rm4x.Tools.Utilities;
using System.IO;
using System.Web;
using System.Web.Http.Controllers;

namespace C4rm4x.WebApi.Security.Acl
{
    internal static class HttpActionContextExtensions
    {
        public static byte[] GetBodyAsByteArray(this HttpActionContext actionContext)
        {
            actionContext.NotNull(nameof(actionContext));

            using (var stream = new MemoryStream())
            {
                var context = actionContext.Request.Properties["MS_HttpContext"] as HttpContextBase;

                if (context.IsNull()) return new byte[0];

                context.Request.InputStream.Seek(0, SeekOrigin.Begin);
                context.Request.InputStream.CopyTo(stream);

                return stream.ToArray();
            }
        }
    }
}
