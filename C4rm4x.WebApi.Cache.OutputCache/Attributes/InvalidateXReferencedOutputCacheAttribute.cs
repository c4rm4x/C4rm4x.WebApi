#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Cache.OutputCache.Internals;
using C4rm4x.WebApi.Framework.Cache;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache
{
    /// <summary>
    /// Invalidate output cache only for the given method that belongs to the 
    /// specified API controller type
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class InvalidateXReferencedOutputCacheAttribute :
        ActionFilterAttribute
    {
        /// <summary>
        /// The name of the action to be invalidated in the cache
        /// </summary>
        public string ActionName { get; private set; }

        /// <summary>
        /// The controller type
        /// </summary>
        public Type ControllerType { get; private set; }

        /// <summary>
        /// Gets the type of the class responsible for generating the keys
        /// </summary>
        public Type CacheKeyGeneratorType { get; private set; } =
            typeof(DefaultCacheKeyGenerator);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controllerType">The controller type (must be ApiController)</param>
        /// <param name="actionName">The action name</param>
        /// <param name="cacheKeyGeneratorType">The type of the class responsible for generating the keys</param>
        public InvalidateXReferencedOutputCacheAttribute(
            Type controllerType,
            string actionName,
            Type cacheKeyGeneratorType = null)
        {
            controllerType.NotNull(nameof(controllerType));
            controllerType.Is<ApiController>();
            actionName.NotNullOrEmpty(nameof(actionName));

            ActionName = actionName;
            ControllerType = controllerType;
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

            await RemoveIfExistsAsync(actionExecutedContext);
        }

        private async Task RemoveIfExistsAsync(
            HttpActionExecutedContext actionExecutedContext)
        {
            var cache = GetCache(actionExecutedContext);
            var cacheKey = GetCacheKey(actionExecutedContext);

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

        private string GetCacheKey(HttpActionExecutedContext actionExecutedContext)
        {
            return GetCacheKeyGenerator()
                .Generate(ControllerType, ActionName, actionExecutedContext.ActionContext);
        }

        private ICacheKeyGenerator GetCacheKeyGenerator()
        {
            return Activator.CreateInstance(CacheKeyGeneratorType)
                as ICacheKeyGenerator;
        }
    }
}
