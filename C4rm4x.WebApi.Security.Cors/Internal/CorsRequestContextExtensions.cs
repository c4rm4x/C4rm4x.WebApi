#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Web.Cors;

#endregion

namespace C4rm4x.WebApi.Security.Cors
{
    internal static class CorsRequestContextExtensions
    {
        public static void SetAccessControlRequestHeaders(
            this CorsRequestContext context,
            string accessControlRequestHeaders)
        {
            if (accessControlRequestHeaders.IsNullOrEmpty()) return;

            var allHeaders = accessControlRequestHeaders
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var header in allHeaders)
                context.AccessControlRequestHeaders.Add(header);
        }
    }
}
