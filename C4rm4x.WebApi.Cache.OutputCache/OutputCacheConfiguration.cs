#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Cache;
using System;
using System.Net.Http;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache
{
    /// <summary>
    /// Class responsible to manage all the configuration output cache related
    /// </summary>
    public class OutputCacheConfiguration
    {
        private readonly HttpConfiguration _config;

        private Func<HttpRequestMessage, ICache> _resolverFactory =
            request => request.GetDependencyScope().GetService(typeof(ICache)) as ICache;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">The Http configuration</param>
        public OutputCacheConfiguration(HttpConfiguration config)
        {
            config.NotNull(nameof(config));

            _config = config;
        }

        /// <summary>
        /// Register the output cache provider 
        /// (in case it differs from the cache for the rest of the application)
        /// </summary>
        /// <param name="provider"></param>
        public void RegisterOutputCacheProvider(Func<ICache> provider)
        {
            provider.NotNull(nameof(provider));

            _config.Properties.GetOrAdd(typeof(ICache), obj => provider);
        }

        /// <summary>
        /// Returns the instance that implements ICache either from the configuration
        /// of the actual dependency scope defined for the whole application
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ICache GetOutputCacheProvider(HttpRequestMessage request)
        {
            object result;

            if (_config.Properties.TryGetValue(typeof(ICache), out result) &&
                result is Func<ICache>)
            {
                var provider = result as Func<ICache>;

                return provider();
            }

            return _resolverFactory(request);
        }

        /// <summary>
        /// Sets the resolver factory
        /// </summary>
        /// <remarks>USE THIS ONLY FOR UNIT TESTING</remarks>
        /// <param name="resolverFactory">The factory</param>
        internal void SetResolverFactory(Func<HttpRequestMessage, ICache> resolverFactory)
        {
            resolverFactory.NotNull(nameof(resolverFactory));

            _resolverFactory = resolverFactory;
        }
    }
}
