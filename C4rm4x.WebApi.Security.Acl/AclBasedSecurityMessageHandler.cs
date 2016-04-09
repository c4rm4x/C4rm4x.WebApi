#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Cache;
using C4rm4x.WebApi.Security.Acl.Internals;
using C4rm4x.WebApi.Security.Acl.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

#endregion

namespace C4rm4x.WebApi.Security.Acl
{
    /// <summary>
    /// Delegating handler responsible to check whether or not the the HTTP requests are
    /// comming form one of the subscribers
    /// </summary>
    public class AclBasedSecurityMessageHandler : 
        SecurityMessageHandler
    {
        private const int OneHour = 60 * 60;

        private Func<AclConfiguration, HttpRequestMessage, ISubscriberRepository> _subscriberRepositoryFactory =
            (config, request) => config.GetSubscriberRepository(request);

        /// <summary>
        /// Gets the actual HttpStatusCode. 
        /// In this case, Unauthorized.
        /// </summary>
        protected override HttpStatusCode ForbiddenErrorCode
        {
            get { return HttpStatusCode.Unauthorized; }
        }

        /// <summary>
        /// Returns whether or not the current HTTP request is allowed to proceeed
        /// </summary>
        /// <param name="request">The current HTTP request</param>
        /// <returns>True if the current HTTP request is allowed; false, otherwise</returns>
        protected override bool IsRequestAllowed(
            HttpRequestMessage request)
        {
            request.NotNull(nameof(request));

            string apiIdentifier, 
                sharedSecret;
            if (!TryRetrieveApiCredentials(request, out apiIdentifier, out sharedSecret))
                return false;

            return ValidateApiCredentials(request, apiIdentifier, sharedSecret);
        }

        private static bool TryRetrieveApiCredentials(
            HttpRequestMessage request,
            out string apiIdentifier,
            out string sharedSecret)
        {
            apiIdentifier = sharedSecret = string.Empty;

            var authorizationAsBase64 = request.Headers.GetAuthorization();

            if (authorizationAsBase64.IsNullOrEmpty())
                return false;

             var authorization = authorizationAsBase64
                .Replace("Basic ", string.Empty)
                .FromBase64()
                .Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

            if (authorization.Length != 2)
                return false;

            return !(apiIdentifier = authorization[0]).IsNullOrEmpty() &&
                !(sharedSecret = authorization[1]).IsNullOrEmpty();
        }

        private bool ValidateApiCredentials(
            HttpRequestMessage request,
            string apiIdentifier,
            string sharedSecret)
        {
            var subscribers = GetSubscribers(request);

            if (subscribers.IsNullOrEmpty()) return false;

            var thisSubscriber = subscribers.FirstOrDefault(
                s => s.Identifier.Equals(apiIdentifier, StringComparison.InvariantCultureIgnoreCase));

            if (thisSubscriber.IsNull()) return false;

            return thisSubscriber.ValidateSecret(sharedSecret);
        }

        private IEnumerable<Subscriber> GetSubscribers(
            HttpRequestMessage request)
        {
            var subscribers = RetrieveFromCache(request);

            if (!subscribers.IsNullOrEmpty()) return subscribers;

            return RetrieveFromRepository(request);
        }

        private IEnumerable<Subscriber> RetrieveFromCache(
            HttpRequestMessage request)
        {
            return GetCache(request)
                .Retrieve<IEnumerable<Subscriber>>(AclConfiguration.SubscribersCacheKey);
        }

        private IEnumerable<Subscriber> RetrieveFromRepository(
            HttpRequestMessage request)
        {
            var subscribers = GetSubscriberRepository(request).GetAll();

            if (!subscribers.IsNullOrEmpty())
                StoreInCache(request, subscribers);

            return subscribers;
        }

        private ISubscriberRepository GetSubscriberRepository(
            HttpRequestMessage request)
        {
            return _subscriberRepositoryFactory(
                GetAclConfiguration(request), 
                request);
        }

        private void StoreInCache(
            HttpRequestMessage request, 
            IEnumerable<Subscriber> subscribers)
        {
            GetCache(request)
                .Store(AclConfiguration.SubscribersCacheKey, subscribers, OneHour);
        }

        private ICache GetCache(HttpRequestMessage request)
        {
            return GetAclConfiguration(request).GetAclCacheProvider(request);
        }

        private AclConfiguration GetAclConfiguration(
            HttpRequestMessage request)
        {
            return request.GetConfiguration().GetAclConfiguration();
        }

        /// <summary>
        /// Sets subscriber repository factory
        /// </summary>
        /// <remarks>USE THIS ONLY UNIT TESTING</remarks>
        /// <param name="subscriberRepositoryFactory">The factory</param>
        internal void SetSubscriberRepositoryFactory(
            Func<AclConfiguration, HttpRequestMessage, ISubscriberRepository> subscriberRepositoryFactory)
        {
            subscriberRepositoryFactory.NotNull(nameof(subscriberRepositoryFactory));

            _subscriberRepositoryFactory = subscriberRepositoryFactory;
        }
    }
}