#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Text;

#endregion

namespace C4rm4x.WebApi.Security.WhiteList.Internals
{
    internal static class StringExtensions
    {
        public static string FromBase64(
            this string base64)
        {
            base64.NotNullOrEmpty(nameof(base64));

            return Encoding.UTF8.GetString(
                Convert.FromBase64String(base64));
        }
    }
}
