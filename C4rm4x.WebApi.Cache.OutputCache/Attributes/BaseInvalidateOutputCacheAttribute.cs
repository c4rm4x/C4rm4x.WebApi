#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Cache;
using System;
using System.Net.Http;
using System.Web.Http;
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
        private ICache _cache;
        
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
        public override void OnActionExecuted(
            HttpActionExecutedContext actionExecutedContext)
        {
            if (!actionExecutedContext.IsASuccessfulResponse()) return;

            InvalidateOutputCache(actionExecutedContext);
        }

        /// <summary>
        /// Invalidate the output cache using the given action executed context
        /// </summary>
        /// <param name="actionExecutedContext">The context</param>
        protected abstract void InvalidateOutputCache(
            HttpActionExecutedContext actionExecutedContext);

        /// <summary>
        /// Removes the entry in the cache (if any) for the given context and action name
        /// </summary>
        /// <param name="actionExecutedContext">The context</param>
        /// <param name="actionName">The action name</param>
        protected void RemoveIfExists(
            HttpActionExecutedContext actionExecutedContext,
            string actionName)
        {
            var cache = GetCache(actionExecutedContext);
            var cacheKey = GetCacheKey(actionExecutedContext, actionName);

            if (!cache.Exists(cacheKey)) return;

            cache.Remove(cacheKey);
        }

        private ICache GetCache(HttpActionExecutedContext actionExecutedContext)
        {
            if (_cache.IsNull())
                _cache = GetCache(
                    actionExecutedContext.Request.GetConfiguration(),
                    actionExecutedContext.Request);

            return _cache;
        }

        private ICache GetCache(
            HttpConfiguration config,
            HttpRequestMessage request)
        {
            return config
                .GetOutputCacheConfiguration()
                .GetOutputCacheProvider(request);
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
