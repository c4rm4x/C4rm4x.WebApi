#region Using

using C4rm4x.Tools.Utilities;
using System.Web.Http.Controllers;
using System;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache
{
    /// <summary>
    /// Default implementation of ICacheKeyGenerator
    /// </summary>
    public class DefaultCacheKeyGenerator : 
        ICacheKeyGenerator
    {
        /// <summary>
        /// Generates the key that should be used to cache/retrieve the content
        /// for the given action context using a combination of controller and action names
        /// </summary>
        /// <param name="actionContext">The action context</param>
        /// <returns>The key for the given action context</returns>
        public virtual string Generate(HttpActionContext actionContext)
        {
            actionContext.NotNull(nameof(actionContext));

            return Generate(actionContext, actionContext.ActionDescriptor.ActionName);
        }

        /// <summary>
        /// Generates the key that should be use to cache/retrieve the content
        /// for the given controllerType and action name
        /// </summary>
        /// <param name="controllerType">The controller type (must be ApiController)</param>
        /// <param name="actionName">The action name</param>
        /// <param name="context">The action context</param>
        /// <returns>The key for the given controller type and action name</returns>
        /// <exception cref="ArgumentException">If controller type is not an ApiController</exception>
        public virtual string Generate(
            Type controllerType,
            string actionName,
            HttpActionContext context)
        {
            controllerType.NotNull(nameof(controllerType));
            controllerType.Is<ApiController>();
            actionName.NotNullOrEmpty(nameof(actionName));
            context.NotNull(nameof(context));

            return "{0}-{1}".AsFormat(controllerType.FullName, actionName);
        }

        /// <summary>
        /// Generates the key that should be used to cache/retrieve the content
        /// for the given action context and action name
        /// </summary>
        /// <param name="actionContext">The action context</param>
        /// <param name="actionName">The action name</param>
        /// <returns>The key for the given action context and action name</returns>
        public virtual string Generate(
            HttpActionContext actionContext, 
            string actionName)
        {
            actionContext.NotNull(nameof(actionContext));
            actionName.NotNullOrEmpty(nameof(actionName));

            return Generate(
                actionContext.ControllerContext.ControllerDescriptor.ControllerType, 
                actionName,
                actionContext);
        }
    }
}
