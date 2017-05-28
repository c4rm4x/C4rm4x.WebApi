#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Threading.Tasks;
using System.Web.Http.Filters;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache
{
    /// <summary>
    /// Invalidate output cache only for the given method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class InvalidateOutputCacheAttribute :
        BaseInvalidateOutputCacheAttribute
    {
        /// <summary>
        /// The name of the action to be invalidated in the cache
        /// </summary>
        public string ActionName { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="actionName">The action name</param>
        /// <param name="cacheKeyGeneratorType">The cache key generator type</param>
        public InvalidateOutputCacheAttribute(
            string actionName,
            Type cacheKeyGeneratorType = null)
            : base(cacheKeyGeneratorType)
        {
            actionName.NotNullOrEmpty(nameof(actionName));

            ActionName = actionName;
        }

        /// <summary>
        /// Invalidate the output cache using the given action executed context
        /// for the given action name (if exists)
        /// </summary>
        /// <param name="actionExecutedContext">The context</param>
        protected override async Task InvalidateOutputCacheAsync(
            HttpActionExecutedContext actionExecutedContext)
        {
            await RemoveIfExistsAsync(actionExecutedContext, ActionName);
        }
    }
}
