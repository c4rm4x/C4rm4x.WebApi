#region Using

using C4rm4x.WebApi.Cache.OutputCache.Internals;
using C4rm4x.WebApi.Framework.Cache;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache
{
    /// <summary>
    /// Base attribute for all the actual invalidate ouput cache attributes (in all their forms)
    /// </summary>
    public abstract class BaseInvalidateOutputCacheAttribute :
        ActionFilterAttribute
    {
        /// <summary>
        /// Gets the type of the class responsible for generating the keys
        /// </summary>
        public Type CacheKeyGeneratorType { get; private set; } =
            typeof(DefaultCacheKeyGenerator);
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cacheKeyGeneratorType">The type of the class responsible for generating the keys</param>
        public BaseInvalidateOutputCacheAttribute(
            Type cacheKeyGeneratorType = null)
        {
            CacheKeyGeneratorType = cacheKeyGeneratorType ?? typeof(DefaultCacheKeyGenerator);
        }

        /// <summary>
        /// Action to occur after the action method is invoked
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        public override async Task OnActionExecutedAsync(
            HttpActionExecutedContext actionExecutedContext,
            CancellationToken cancellationToken)
        {
            if (!actionExecutedContext.IsASuccessfulResponse()) return;

            await InvalidateOutputCacheAsync(actionExecutedContext);
        }

        /// <summary>
        /// Invalidate the output cache using the given action executed context
        /// </summary>
        /// <param name="actionExecutedContext">The context</param>
        protected abstract Task InvalidateOutputCacheAsync(
            HttpActionExecutedContext actionExecutedContext);

        /// <summary>
        /// Removes the entry in the cache (if any) for the given context and action name
        /// </summary>
        /// <param name="actionExecutedContext">The context</param>
        /// <param name="actionName">The action name</param>
        protected async Task RemoveIfExistsAsync(
            HttpActionExecutedContext actionExecutedContext,
            string actionName)
        {
            var cache = GetCache(actionExecutedContext);
            var cacheKey = GetCacheKey(actionExecutedContext, actionName);

            if (!await cache.ExistsAsync(cacheKey)) return;

            await cache.RemoveAsync(cacheKey);
        }

        private ICache GetCache(HttpActionExecutedContext actionExecutedContext)
        {
            return actionExecutedContext
                .Request
                .GetConfiguration()
                .GetOutputCacheConfiguration()
                .GetOutputCacheProvider(actionExecutedContext.Request);
        }

        private string GetCacheKey(
            HttpActionExecutedContext actionExecutedContext, 
            string actionName)
        { 
            return GetCacheKeyGenerator()
                .Generate(actionExecutedContext.ActionContext, actionName);
        }

        private ICacheKeyGenerator GetCacheKeyGenerator()
        {
            return Activator.CreateInstance(CacheKeyGeneratorType)
                as ICacheKeyGenerator;
        }
    }
}
