#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Cache;
using System;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Security.Acl
{
    /// <summary>
    /// Message handler extensions
    /// </summary>
    public static class MessageHandlerExtensions
    {
        /// <summary>
        /// Configure message handlers to enable ACL based 
        /// SecurityMessageHandler for all the requests
        /// </summary>
        /// <param name="config">The config</param>
        /// <param name="forceAuthentication">Indicate whether or not authentication must be enforced</param>
        /// <param name="cacheProvider">Sets the cache provider (if this differs for the one used in the rest of the application)</param>
        public static void EnableAcl(
            this HttpConfiguration config,
            bool forceAuthentication = false,
            Func<ICache> cacheProvider = null)
        {
            config.NotNull(nameof(config));

            config.MessageHandlers.Add(
                new AclBasedSecurityMessageHandler(forceAuthentication));

            SetAclCacheProvider(config, cacheProvider);
        }

        private static void SetAclCacheProvider(
            HttpConfiguration config,
            Func<ICache> cacheProvider)
        {
            if (cacheProvider.IsNull()) return;

            config
                .GetAclConfiguration()
                .RegisterAclCacheProvider(cacheProvider);
        }
    }
}
