#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Cache;
using System;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Security.WhiteList
{
    /// <summary>
    /// Message handler extensions
    /// </summary>
    public static class MessageHandlerExtensions
    {
        /// <summary>
        /// Configure message handlers to enable white list based 
        /// SecurityMessageHandler for all the requests
        /// </summary>
        /// <param name="config">The config</param>
        /// <param name="cacheProvider">Sets the cache provider (if this differs for the one used in the rest of the application)</param>
        public static void EnableWhiteList(
            this HttpConfiguration config,
            Func<ICache> cacheProvider = null)
        {
            config.NotNull(nameof(config));

            config.MessageHandlers.Add(
                new WhiteListBasedSecurityMessageHandler());

            SetWhiteListCacheProvider(config, cacheProvider);
        }

        private static void SetWhiteListCacheProvider(
            HttpConfiguration config,
            Func<ICache> cacheProvider)
        {
            if (cacheProvider.IsNull()) return;

            config
                .GetWhiteListConfiguration()
                .RegisterWhiteListCacheProvider(cacheProvider);
        }
    }
}
