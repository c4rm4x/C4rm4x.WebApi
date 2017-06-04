#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Cache.Redis;
using C4rm4x.WebApi.Framework.Cache;
using System;
using System.Threading.Tasks;
using BaseAbstractServiceStatusRetriever = C4rm4x.WebApi.Monitoring.ServiceStatus.AbstractServiceStatusRetriever;

#endregion

namespace C4rm4x.WebApi.Monitoring.Redis
{
    /// <summary>
    /// Basic implementation of IServiceStatusRetriever using Redis
    /// </summary>
    public abstract class AbstractServiceStatusRetriever
        : BaseAbstractServiceStatusRetriever
    {
        private readonly ICache _cache;
         
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="componentIdentifier">The component's identifier</param>
        /// <param name="componentName">The component's name</param>
        /// <param name="cache">The cache</param>
        /// <remarks>The cache must be an instace of RedisCache</remarks>
        public AbstractServiceStatusRetriever(
            object componentIdentifier, 
            string componentName,
            ICache cache) : 
            base(componentIdentifier, componentName)
        {
            cache.NotNull(nameof(cache));
            cache.GetType().Is<RedisCache>();

            _cache = cache;
        }

        /// <summary>
        /// Checks whether or not the Redis instance is up and running
        /// </summary>
        protected override async Task CheckComponentResponsivenessAsync()
        {
            var Key = GenerateKey();
            var Value = "Test";

            await StoreAsync(Key, Value)
                .ContinueWith((t) => RetrieveAsync(Key, Value));
        }

        private static string GenerateKey()
        {
            return DateTime.UtcNow.ToString();
        }

        private Task StoreAsync(
            string key,
            string value)
        {
            return _cache.StoreAsync(key, value, 5);
        }

        private async Task RetrieveAsync(
            string key, 
            string value)
        {
            var retrievedValue = await _cache.RetrieveAsync(key) as string;

            value.Must(
                s => s.Equals(retrievedValue), 
                "Both values must be equal. Expected = {0}, Actual = {1}".AsFormat(value, retrievedValue));
        }
    }
}
