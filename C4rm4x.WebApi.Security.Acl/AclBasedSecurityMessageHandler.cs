#region Using

using C4rm4x.Tools.Security.Acl;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Cache;
using C4rm4x.WebApi.Security.Acl.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

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

        private Action<HttpRequestMessage, IPrincipal> _assignPrincipalFactory =
            (request, principal) => request.GetRequestContext().Principal = principal;

        /// <summary>
        /// Gets the actual HttpStatusCode. 
        /// In this case, Unauthorized.
        /// </summary>
        protected override HttpStatusCode ForbiddenErrorCode
        {
            get { return HttpStatusCode.Unauthorized; }
        }

        /// <summary>
        /// Gets whether or not the header must be present for the request to be processed
        /// </summary>
        public bool ForceAuthentication { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="forceAuthentication">Indicates whether or not the header must be present for the request to be processed</param>
        public AclBasedSecurityMessageHandler(
            bool forceAuthentication)
        {
            ForceAuthentication = forceAuthentication;
        }

        /// <summary>
        /// Returns whether or not the current HTTP request is allowed to proceeed
        /// </summary>
        /// <param name="request">The current HTTP request</param>
        /// <returns>True if the current HTTP request is allowed; false, otherwise</returns>
        protected override async Task<bool> IsRequestAllowedAsync(
            HttpRequestMessage request)
        {
            request.NotNull(nameof(request));

            AclClientCredentials credentials;
            if (!TryRetrieveApiCredentials(request, out credentials))
                return !ForceAuthentication;

            return await ValidateApiCredentialsAsync(request, credentials);
        }

        private static bool TryRetrieveApiCredentials(
            HttpRequestMessage request,
            out AclClientCredentials credentials)
        {
            return new AclClientCredentialsRetriever()
                .TryParse(request, out credentials);
        }

        private async Task<bool> ValidateApiCredentialsAsync(
            HttpRequestMessage request,
            AclClientCredentials credentials)
        {
            var subscribers = await GetSubscribersAsync(request);

            if (subscribers.IsNullOrEmpty()) return false;

            var thisSubscriber = subscribers.FirstOrDefault(
                s => s.Identifier.Equals(credentials.Identifier, StringComparison.InvariantCultureIgnoreCase));

            if (thisSubscriber.IsNull()) return false;

            IPrincipal principal;
            var result = thisSubscriber
                .ValidateCredentials(credentials, out principal);

            if (result)
            {
                Thread.CurrentPrincipal = principal;
                _assignPrincipalFactory(request, principal);
            }

            return result;
        }

        private async Task<IEnumerable<Subscriber>> GetSubscribersAsync(
            HttpRequestMessage request)
        {
            var subscribers = await RetrieveFromCacheAsync(request);

            if (!subscribers.IsNullOrEmpty()) return subscribers;

            return await RetrieveFromRepositoryAsync(request);
        }

        private Task<IEnumerable<Subscriber>> RetrieveFromCacheAsync(
            HttpRequestMessage request)
        {
            return GetCache(request)
                .RetrieveAsync<IEnumerable<Subscriber>>(AclConfiguration.SubscribersCacheKey);
        }

        private async Task<IEnumerable<Subscriber>> RetrieveFromRepositoryAsync(
            HttpRequestMessage request)
        {
            var subscribers = await GetSubscriberRepository(request).GetAllAsync();

            if (!subscribers.IsNullOrEmpty())
                await StoreInCacheAsync(request, subscribers);

            return subscribers;
        }

        private ISubscriberRepository GetSubscriberRepository(
            HttpRequestMessage request)
        {
            return _subscriberRepositoryFactory(
                GetAclConfiguration(request), 
                request);
        }

        private Task StoreInCacheAsync(
            HttpRequestMessage request, 
            IEnumerable<Subscriber> subscribers)
        {
            return GetCache(request)
                .StoreAsync(AclConfiguration.SubscribersCacheKey, subscribers, OneHour);
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
        /// Sets the subscriber repository factory
        /// </summary>
        /// <remarks>USE THIS ONLY UNIT TESTING</remarks>
        /// <param name="subscriberRepositoryFactory">The factory</param>
        internal void SetSubscriberRepositoryFactory(
            Func<AclConfiguration, HttpRequestMessage, ISubscriberRepository> subscriberRepositoryFactory)
        {
            subscriberRepositoryFactory.NotNull(nameof(subscriberRepositoryFactory));

            _subscriberRepositoryFactory = subscriberRepositoryFactory;
        }

        /// <summary>
        /// Sets the assign principal factory
        /// </summary>
        /// <remarks>USE THIS ONLY UNIT TESTING</remarks>
        /// <param name="assignPrincipalFactory">The factory</param>
        internal void SetAssignPrincipalFactory(
            Action<HttpRequestMessage, IPrincipal> assignPrincipalFactory)
        {
            assignPrincipalFactory.NotNull(nameof(assignPrincipalFactory));

            _assignPrincipalFactory = assignPrincipalFactory;
        }
    }
}