#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Cache;
using StackExchange.Redis;
using System;

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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        public RedisCache(string connectionString)
        {
            connectionString.NotNullOrEmpty(nameof(connectionString));

            ConnectionString = connectionString;
        }

        private Lazy<ConnectionMultiplexer> Connection =>
            new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(ConnectionString);
            });

        private IDatabase Cache
        {
            get { return Connection.Value.GetDatabase(); }
        }

        /// <summary>
        /// Retrieves an object from the cache based on specified key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The object based on specified key if exists. Otherwise null</returns>
        public object Retrieve(string key)
        {
            key.NotNullOrEmpty(nameof(key));

            string value = Cache.StringGet(key);

            return value;
        }

        /// <summary>
        /// Retrieves an object of type T from the cache 
        /// based on specified key
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve</typeparam>
        /// <param name="key">The key</param>
        /// <returns>The object based on spefified key if exists. Otherwise null</returns>
        /// <exception cref="InvalidCastException">When object cannot be cast to specified type</exception>
        public T Retrieve<T>(string key)
        {
            var valueCached = Retrieve(key) as string; // It is always string

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
        public void Store(
            string key, 
            object objectToStore, 
            int expirationTime = 60)
        {
            key.NotNullOrEmpty(nameof(key));
            objectToStore.NotNull(nameof(objectToStore));

            Cache.StringSet(key, objectToStore.SerializeAsString(), GetExpirationTime(expirationTime));
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
        public bool Exists(string key)
        {
            return Cache.KeyExists(key);
        }

        /// <summary>
        /// Removes the entry cached associated with the given key (if any)
        /// </summary>
        /// <param name="key">They key</param>
        public void Remove(string key)
        {
            Cache.KeyDelete(key);
        }
    }
}
