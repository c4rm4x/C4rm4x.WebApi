﻿#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Cache.OutputCache.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache
{
    /// <summary>
    /// Invalidate output cache for all the get-methods within the controller associated
    /// to this context for all POST, PUT and DELETE http methods sucessfully processed
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class AutoInvalidateOutputCacheAttribute :
        BaseInvalidateOutputCacheAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cacheKeyGeneratorType">The cache key generator type</param>
        public AutoInvalidateOutputCacheAttribute(
            Type cacheKeyGeneratorType = null)
            : base(cacheKeyGeneratorType)
        { }

        /// <summary>
        /// Invalidate the output cache using the given action executed context
        /// for all the get-methods for all POST, PUT and DELETE http methods successfully processed
        /// </summary>
        /// <param name="actionExecutedContext">The context</param>
        protected override async Task InvalidateOutputCacheAsync(
            HttpActionExecutedContext actionExecutedContext)
        {
            if (!actionExecutedContext.MayRequestModifyResult()) return;

            var allGetActionNames = FindAllGetActionNames(actionExecutedContext);

            if (allGetActionNames.IsNullOrEmpty()) return;

            var tasks = allGetActionNames.Select(actionName => RemoveIfExistsAsync(actionExecutedContext, actionName));
            await Task.WhenAll(tasks);
        }

        private static IEnumerable<string> FindAllGetActionNames(
            HttpActionExecutedContext actionExecutedContext)
        {
            var allActions = actionExecutedContext
                .ActionContext
                .ControllerContext
                .ControllerDescriptor
                .ControllerType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            return ProjectActionNames(allActions.Where(IsGetAction));
        }

        private static bool IsGetAction(MethodInfo action)
        {
            return action.Name.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase) ||
                action.GetCustomAttributes<HttpGetAttribute>(true).Any();
        }

        private static IEnumerable<string> ProjectActionNames(
            IEnumerable<MethodInfo> actions)
        {
            return actions.Select(GetActualName);
        }

        private static string GetActualName(MethodInfo action)
        {
            var overridenNames = action.GetCustomAttributes<ActionNameAttribute>(false);

            return overridenNames.Any()
                ? overridenNames.First().Name
                : action.Name;
        }
    }
}
