#region Using

using C4rm4x.Tools.Utilities;
using CloudinaryDotNet.Actions;
using System;
using System.Net;

#endregion

namespace C4rm4x.WebApi.Storage.Cloudinary
{
    internal static class Extensions
    {
        public static TResult EnsureSuccessStatusCode<TResult>(
            this TResult result)
            where TResult : BaseResult
        {
            result.NotNull(nameof(result));

            if (result.StatusCode != HttpStatusCode.OK &&
                result.StatusCode != HttpStatusCode.Created &&
                result.StatusCode != HttpStatusCode.Accepted)
                throw new ArgumentException("Status code does not represent a success");

            return result;
        }
    }
}
