#region Using

using C4rm4x.Tools.Utilities;
using System.Web.Http.Controllers;

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
        public string Generate(HttpActionContext actionContext)
        {
            actionContext.NotNull(nameof(actionContext));

            return "{0}-{1}".AsFormat(
                actionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName.ToLower(),
                actionContext.ActionDescriptor.ActionName.ToLower());
        }
    }
}
