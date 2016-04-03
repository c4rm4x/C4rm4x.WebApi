#region Using

using C4rm4x.Tools.Utilities;
using System.Net.Http;
using System.Web.Http.Filters;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache
{
    internal static class HttpActionExecutedContextExtensions
    {
        public static bool IsASuccessfulResponse(
            this HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.NotNull(nameof(actionExecutedContext));

            return actionExecutedContext.ActionContext.Response.IsNotNull() &&
                actionExecutedContext.ActionContext.Response.IsSuccessStatusCode;
        }

        public static bool MayRequestModifyResult(
            this HttpActionExecutedContext actionExecutedContext)
        {
            var method = actionExecutedContext.ActionContext.Request.Method;

            return method == HttpMethod.Post ||
                method == HttpMethod.Put ||
                method == HttpMethod.Delete;
        }
    }
}
