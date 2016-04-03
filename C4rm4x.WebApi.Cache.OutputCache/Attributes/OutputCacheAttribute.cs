#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Cache.OutputCache.Internals;
using C4rm4x.WebApi.Framework.Cache;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache
{
    /// <summary>
    /// Base Ouput Cache attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class OutputCacheAttribute : 
        ActionFilterAttribute
    {
        private ICache _cache;

        private Func<DateTime> _nowFactory = () => DateTime.UtcNow;

        /// <summary>
        /// Gets how long the output should be cached in the server (in seconds)
        /// </summary>
        public int ServerTimeSpan { get; private set; }

        /// <summary>
        /// Gets how long the output should be cached in the browser (in seconds)
        /// </summary>
        public int ClientTimeSpan { get; private set; }

        /// <summary>
        /// Gets the type of the class responsible for generating the keys
        /// </summary>
        public Type CacheKeyGeneratorType { get; private set; } = 
            typeof(DefaultCacheKeyGenerator);

        private CacheTime CacheTime => 
            CacheTime.From(ServerTimeSpan, ClientTimeSpan, _nowFactory());

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverTimeSpan">The time the output must be cached in the server (in seconds)</param>
        /// <param name="clientTimeSpan">The time the output can be cached in the browser (in seconds)</param>
        /// <param name="cacheKeyGeneratorType">The type of the class responsible for generating the keys</param>
        public OutputCacheAttribute(
            int serverTimeSpan, 
            int clientTimeSpan = 0,
            Type cacheKeyGeneratorType = null)
        {
            serverTimeSpan.Must(x => x > 0, "serverTimeSpan must be greater than 0");
            clientTimeSpan.Must(x => x >= 0, "clientTimeSpan must be equal or greater than 0");

            ServerTimeSpan = serverTimeSpan;
            ClientTimeSpan = clientTimeSpan;
            CacheKeyGeneratorType = cacheKeyGeneratorType ?? typeof(DefaultCacheKeyGenerator);
        }                

        /// <summary>
        /// Action to occur before the actual action method is invoked
        /// </summary>
        /// <param name="actionContext">The action context</param>
        public override void OnActionExecuting(
            HttpActionContext actionContext)
        {
            actionContext.NotNull(nameof(actionContext));

            if (!IsCachingAllowed(actionContext)) return;

            var content = GetCachedContent(actionContext);

            if (content.IsNullOrEmpty()) return;

            CreateResponse(actionContext, content);
            
            ApplyCacheHeaders(actionContext.Response);
        }

        private byte[] GetCachedContent(
            HttpActionContext actionContext)
        {
            return GetCache(actionContext)
                .Retrieve<byte[]>(GetCacheKey(actionContext));
        }

        private static void CreateResponse(
            HttpActionContext actionContext,
            byte[] content)
        {
            const string JsonContentType = "application/json";

            actionContext.Response = actionContext.Request.CreateResponse();
            actionContext.Response.Content = new ByteArrayContent(content);
            actionContext.Response.Content.Headers.ContentType = new MediaTypeHeaderValue(JsonContentType);
        }

        /// <summary>
        /// Action to occur after the action method is invoked
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context</param>
        public override void OnActionExecuted(
            HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.NotNull(nameof(actionExecutedContext));

            if (!actionExecutedContext.IsASuccessfulResponse()) return;

            if (!IsCachingAllowed(actionExecutedContext.ActionContext)) return;

            if (CacheTime.AbsoluteExpirationTime <= _nowFactory()) return;

            var cacheKey = GetCacheKey(actionExecutedContext.ActionContext);

            if (cacheKey.IsNullOrEmpty() ||
                ExistsItemInCache(actionExecutedContext, cacheKey) ||
                actionExecutedContext.Response.Content.IsNull())
                return;

            StoreContent(actionExecutedContext, cacheKey);

            ApplyCacheHeaders(actionExecutedContext.Response);
        }

        private bool ExistsItemInCache(
            HttpActionExecutedContext actionExecutedContext, 
            string cacheKey)
        {
            return GetCache(actionExecutedContext.ActionContext)
                .Exists(cacheKey);
        }

        private void StoreContent(
            HttpActionExecutedContext actionExecutedContext,
            string cacheKey)
        {
            var actionContext = actionExecutedContext.ActionContext;
            var content = actionExecutedContext
                .Response.Content.ReadAsByteArrayAsync().Result;

            GetCache(actionContext).Store(cacheKey, content, ServerTimeSpan);
        }

        /// <summary>
        /// Returns whether or not the caching is allowed for the given action context
        /// </summary>
        /// <param name="actionContext">The action context</param>
        /// <returns>True if the request method is a GET; false, otherwise</returns>
        protected virtual bool IsCachingAllowed(HttpActionContext actionContext)
        {
            return actionContext.Request.Method == HttpMethod.Get;
        }

        private ICache GetCache(HttpActionContext actionContext)
        {
            if (_cache.IsNull())
                _cache = GetCache(
                    actionContext.Request.GetConfiguration(),
                    actionContext.Request);

            return _cache;
        }

        private ICache GetCache(
            HttpConfiguration config,
            HttpRequestMessage request)
        {
            return config
                .GetOutputCacheConfiguration()
                .GetOutputCacheProvider(request);
        }

        private string GetCacheKey(HttpActionContext actionContext)
        {
            return GetCacheKeyGenerator().Generate(actionContext);
        }

        private ICacheKeyGenerator GetCacheKeyGenerator()
        {
            return Activator.CreateInstance(CacheKeyGeneratorType) 
                as ICacheKeyGenerator;
        }

        /// <summary>
        /// Sets the cache headers
        /// </summary>
        /// <param name="response">The actual response</param>
        protected virtual void ApplyCacheHeaders(
            HttpResponseMessage response)
        {
            if (CacheTime.ClientTimeSpan <= TimeSpan.Zero) return;

            var cacheControl = new CacheControlHeaderValue
            {
                MaxAge = CacheTime.ClientTimeSpan
            };

            response.Headers.CacheControl = cacheControl;
        }

        /// <summary>
        /// Sets the now factory
        /// </summary>
        /// <param name="nowFactory">The factory</param>
        /// <remarks>USE THIS ONLY FOR UNIT TESTING</remarks>
        internal void SetNowFactory(Func<DateTime> nowFactory)
        {
            nowFactory.NotNull(nameof(nowFactory));

            _nowFactory = nowFactory;
        }
    }
}
