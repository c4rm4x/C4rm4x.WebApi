#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework;
using C4rm4x.WebApi.Framework.Cache;
using System;
using System.Threading.Tasks;
using System.Web.Caching;

#endregion

namespace C4rm4x.WebApi.Cache.HttpRuntime
{
    /// <summary>
    /// Implements a service to cache information for a Web Application
    /// using System.Web.HttpRuntime.Cache
    /// </summary>
    [DomainService(typeof(ICache))]
    public class Cache : ICache
    {
        private DateTime NoAbsoluteExpiration => 
            System.Web.Caching.Cache.NoAbsoluteExpiration;

        private TimeSpan NoSlidingExpiration =>
            System.Web.Caching.Cache.NoSlidingExpiration;

        private System.Web.Caching.Cache HttpCache => 
            System.Web.HttpRuntime.Cache;

        /// <summary>
        /// Retrieves an object from the cache based on specified key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The object based on specified key if exists. Otherwise null</returns>
        public async Task<object> RetrieveAsync(string key)
        {
            key.NotNullOrEmpty(nameof(key));

            return await Task.FromResult(HttpCache.Get(key));
        }

        /// <summary>
        /// Stores an object in the cache linked to specified key for a period of time
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="objectToStore">The object to store</param>
        /// <param name="expirationTime">The time for which the object will be stored (-1 if you want the object not to expire) in seconds</param>
        /// <remarks>
        /// If a previous object exists with the specified key
        /// it will be overwritten
        /// </remarks>
        public async Task StoreAsync(
            string key,
            object objectToStore,
            int expirationTime = 60)
        {
            key.NotNullOrEmpty(nameof(key));
            objectToStore.NotNull(nameof(objectToStore));

            await Task.FromResult(HttpCache.Add(
                key,
                objectToStore,
                null,
                GetExpirationTime(expirationTime),
                NoSlidingExpiration,
                CacheItemPriority.Normal, null));
        }

        private DateTime GetExpirationTime(int expirationTime)
        {
            return expirationTime == -1
                ? NoAbsoluteExpiration
                : DateTime.Now.AddSeconds(expirationTime);
        }

        /// <summary>
        /// Retrieves an object of type T from the cache 
        /// based on specified key
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve</typeparam>
        /// <param name="key">The key</param>
        /// <returns>The object based on spefified key if exists. Otherwise null</returns>
        /// <exception cref="InvalidCastException">When object cannot be cast to specified type</exception>
        public async Task<T> RetrieveAsync<T>(string key)
        {
            return (T)await RetrieveAsync(key);
        }

        /// <summary>
        /// Retrieves whether or not there is an entry cached with the given key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>True when there is an entry stored with the given key; false, otherwise</returns>
        public async Task<bool> ExistsAsync(string key)
        {
            var result = await RetrieveAsync(key);

            return result.IsNotNull();
        }

        /// <summary>
        /// Removes the entry cached associated with the given key (if any)
        /// </summary>
        /// <param name="key">The key</param>
        public async Task RemoveAsync(string key)
        {
            await Task.FromResult(HttpCache.Remove(key));
        }
    }
}
