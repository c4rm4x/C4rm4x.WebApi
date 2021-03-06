﻿#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Cache;
using C4rm4x.WebApi.Security.Acl.Subscriptions;
using System;
using System.Net.Http;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Security.Acl
{
    /// <summary>
    /// Class responsible to manage all the configuration ACL related
    /// </summary>
    public class AclConfiguration
    {
        /// <summary>
        /// The key associated with the list of susbcribers in the cache
        /// </summary>
        internal const string SubscribersCacheKey =
            "AclSecurityMessageHandler.Subscribers";

        private readonly HttpConfiguration _config;

        private Func<HttpRequestMessage, Type, object> _resolverFactory =
            (request, type) => request.GetDependencyScope().GetService(type);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">The Http configuration</param>
        public AclConfiguration(HttpConfiguration config)
        {
            config.NotNull(nameof(config));

            _config = config;
        }

        /// <summary>
        /// Register the ACL cache provider 
        /// (in case it differs from the cache for the rest of the application)
        /// </summary>
        /// <param name="provider"></param>
        public void RegisterAclCacheProvider(Func<ICache> provider)
        {
            provider.NotNull(nameof(provider));

            _config.Properties.GetOrAdd(typeof(ICache), obj => provider);
        }

        /// <summary>
        /// Returns the instance that implements ICache either from the configuration
        /// of the actual dependency scope defined for the whole application
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>The instance that implements ICache</returns>
        public ICache GetAclCacheProvider(HttpRequestMessage request)
        {
            object result;

            if (_config.Properties.TryGetValue(typeof(ICache), out result) &&
                result is Func<ICache>)
            {
                var provider = result as Func<ICache>;

                return provider();
            }

            return _resolverFactory(request, typeof(ICache)) as ICache;
        }

        /// <summary>
        /// Returns the instance that implements ISubscriptionDataProvider defined 
        /// for the whole application
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>The instance that implements ISubscriptionDataProvider</returns>
        public ISubscriberRepository GetSubscriberRepository(
            HttpRequestMessage request)
        {
            return _resolverFactory(request, typeof(ISubscriberRepository)) 
                as ISubscriberRepository;
        }

        /// <summary>
        /// Sets the resolver factory
        /// </summary>
        /// <remarks>USE THIS ONLY FOR UNIT TESTING</remarks>
        /// <param name="resolverFactory">The factory</param>
        internal void SetResolverFactory(Func<HttpRequestMessage, Type, object> resolverFactory)
        {
            resolverFactory.NotNull(nameof(resolverFactory));

            _resolverFactory = resolverFactory;
        }
    }
}
