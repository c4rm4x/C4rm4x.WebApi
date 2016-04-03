#region Using

using System.Web.Http.Controllers;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache
{
    /// <summary>
    /// Generator of keys to use for caching/retrieving the content for a given
    /// action context
    /// </summary>
    public interface ICacheKeyGenerator
    {
        /// <summary>
        /// Generates the key that should be used to cache/retrieve the content
        /// for the given action context
        /// </summary>
        /// <param name="actionContext">The action context</param>
        /// <returns>The key for the given action context</returns>
        string Generate(HttpActionContext actionContext);
    }
}
