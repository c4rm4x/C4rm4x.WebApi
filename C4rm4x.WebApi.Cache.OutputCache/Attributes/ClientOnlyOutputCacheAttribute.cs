#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Cache.OutputCache.Internals;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache
{
    /// <summary>
    /// Client only cache attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ClientOnlyOutputCacheAttribute :
        ActionFilterAttribute
    {
        /// <summary>
        /// Gets how long the output should be cached in the browser (in seconds)
        /// </summary>
        public int ClientTimeSpan { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientTimeSpan">The time the output can be cached in the browser (in seconds)</param>
        public ClientOnlyOutputCacheAttribute(
            int clientTimeSpan)
        {
            clientTimeSpan.Must(x => x > 0, "clientTimeSpan must be greater than 0");

            ClientTimeSpan = clientTimeSpan;
        }

        /// <summary>
        /// Action to occur after the action method is invoked
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.NotNull(nameof(actionExecutedContext));

            if (!actionExecutedContext.IsASuccessfulResponse()) return;

            if (!IsCachingAllowed(actionExecutedContext.ActionContext)) return;

            if (actionExecutedContext.Response.Content.IsNull()) return;

            ApplyCacheHeaders(actionExecutedContext.Response);
        }

        /// <summary>
        /// Returns whether or not the caching is allowed for the given action context
        /// </summary>
        /// <param name="actionContext">The action context</param>
        /// <returns>True if the request method is a GET; false, otherwise</returns>
        protected virtual bool IsCachingAllowed(HttpActionContext actionContext)
        {
            return actionContext.Request.Method == HttpMethod.Get;
        }

        /// <summary>
        /// Sets the cache headers
        /// </summary>
        /// <param name="response">The actual response</param>
        protected virtual void ApplyCacheHeaders(
            HttpResponseMessage response)
        {
            var cacheControl = new CacheControlHeaderValue
            {
                MaxAge = TimeSpan.FromSeconds(ClientTimeSpan)
            };

            response.Headers.CacheControl = cacheControl;
        }
    }
}
