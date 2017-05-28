#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Cache;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Cache.Redis
{
    /// <summary>
    /// Implementation of ICache using Redis as cache mechanism
    /// </summary>
    public class RedisCache : ICache
    {
        /// <summary>
        /// Gets the connection string for your Redis instance
        /// </summary>
        public string ConnectionString { get; private set; }

        private Lazy<ConnectionMultiplexer> Connection { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        public RedisCache(string connectionString)
        {
            connectionString.NotNullOrEmpty(nameof(connectionString));

            ConnectionString = connectionString;

            Connection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(ConnectionString);
            });
        }

        private IDatabase Cache
        {
            get { return Connection.Value.GetDatabase(); }
        }

        /// <summary>
        /// Retrieves an object from the cache based on specified key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The object based on specified key if exists. Otherwise null</returns>
        public async Task<object> RetrieveAsync(string key)
        {
            key.NotNullOrEmpty(nameof(key));

            string result = await Cache.StringGetAsync(key);

            return result;
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
            var valueCached = await RetrieveAsync(key) as string; // It is always string

            if (valueCached.IsNullOrEmpty()) return default(T);

            return valueCached.DeserializeAs<T>();
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

            await Cache.StringSetAsync(key, objectToStore.SerializeAsString(), GetExpirationTime(expirationTime));
        }

        private static TimeSpan? GetExpirationTime(int expirationTime)
        {
            return expirationTime == -1
                ? (TimeSpan?)null
                : new TimeSpan(0, 0, expirationTime);
        }

        /// <summary>
        /// Retrieves whether or not there is an entry cached with the given key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>True when there is an entry stored with the given key; false, otherwise</returns>
        public async Task<bool> ExistsAsync(string key)
        {
            return await Cache.KeyExistsAsync(key);
        }

        /// <summary>
        /// Removes the entry cached associated with the given key (if any)
        /// </summary>
        /// <param name="key">They key</param>
        public async Task RemoveAsync(string key)
        {
            await Cache.KeyDeleteAsync(key);
        }
    }
}
