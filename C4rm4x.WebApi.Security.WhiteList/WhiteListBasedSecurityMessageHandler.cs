#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Cache;
using C4rm4x.WebApi.Security.WhiteList.Internals;
using C4rm4x.WebApi.Security.WhiteList.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

#endregion

namespace C4rm4x.WebApi.Security.WhiteList
{
    /// <summary>
    /// Delegating handler responsible to check whether or not the the HTTP requests are
    /// comming form one of the subscribers
    /// </summary>
    public class WhiteListBasedSecurityMessageHandler : 
        SecurityMessageHandler
    {
        private const int OneHour = 60 * 60;        

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

            var authorization = request.Headers.GetAuthorization()
                .FromBase64()
                .Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

            if (authorization.IsNullOrEmpty() || authorization.Length != 2)
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

            return RetrieveFromDataProvider(request);
        }

        private IEnumerable<Subscriber> RetrieveFromCache(
            HttpRequestMessage request)
        {
            return GetCache(request)
                .Retrieve<IEnumerable<Subscriber>>(WhiteListConfiguration.SubscribersCacheKey);
        }

        private IEnumerable<Subscriber> RetrieveFromDataProvider(
            HttpRequestMessage request)
        {
            var subscribers = GetSubscriptionDataProvider(request).GetAll();

            if (!subscribers.IsNullOrEmpty())
                StoreInCache(request, subscribers);

            return subscribers;
        }

        private ISubscriptionDataProvider GetSubscriptionDataProvider(
            HttpRequestMessage request)
        {
            return GetWhiteListConfiguration(request)
                .GetSubscriptionDataProvider(request);
        }

        private void StoreInCache(
            HttpRequestMessage request, 
            IEnumerable<Subscriber> subscribers)
        {
            GetCache(request)
                .Store(WhiteListConfiguration.SubscribersCacheKey, subscribers, OneHour);
        }

        private ICache GetCache(HttpRequestMessage request)
        {
            return GetWhiteListConfiguration(request)
                .GetWhiteListCacheProvider(request);
        }

        private WhiteListConfiguration GetWhiteListConfiguration(
            HttpRequestMessage request)
        {
            return request
                .GetConfiguration()
                .GetWhiteListConfiguration();
        }
    }
}