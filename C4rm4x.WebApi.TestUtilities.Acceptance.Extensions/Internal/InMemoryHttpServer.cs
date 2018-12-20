#region Using

using C4rm4x.Tools.HttpUtilities;
using C4rm4x.Tools.Utilities;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.TestUtilities.Acceptance.Internal
{
    /// <summary>
    /// Service responsible to manage all the interaction between the acceptance tests and in-memory server
    /// </summary>
    internal class InMemoryHttpServer : IDisposable
    {
        private IDisposable _selfthostedWebApi;

        /// <summary>
        /// Gets the url this in-memory http server in bound to
        /// </summary>
        public string BaseUrl { get; private set; }

        /// <summary>
        /// Gets whether or not this instance has already been disposed
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets the security token to be added as request Authorization header
        /// </summary>
        public string SecurityToken { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public InMemoryHttpServer()
        {
            BaseUrl = ConfigurationManager.AppSettings["Acceptance.Test.Server.BaseUrl"];
        }

        /// <summary>
        /// Configure the host with the given middleware
        /// </summary>
        /// <param name="withMiddleware">The middleware</param>
        public void Configure(params Action<HttpConfiguration>[] withMiddleware)
        {
            withMiddleware.NotNull(nameof(withMiddleware));

            _selfthostedWebApi = WebApp.Start(BaseUrl, appBuilder =>
            {
                var config = new HttpConfiguration();

                foreach (var middleware in withMiddleware) middleware(config);

                config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

                appBuilder.UseWebApi(config);
            });
        }

        /// <summary>
        /// Returns the Http response from a Get request
        /// </summary>
        /// <param name="method">Method to execute to retrieve instance</param>
        /// <param name="customHeaders">Custom headers to be added as part of request headers</param>
        /// <param name="parameters">Parameters to include as part of query string</param>
        /// <returns>Returns the HttpResponseMessage</returns>
        public Task<HttpResponseMessage> GetAsync(
            string method,
            Action<HttpRequestHeaders> customHeaders = null,
            params KeyValuePair<string, object>[] parameters)
        {
            return RESTfulConsumer.GetAsync(BaseUrl, method, IncludeAllHeaders(customHeaders), parameters);
        }

        /// <summary>
        /// Returns an instance of type T
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="method">Method to execute to retrieve instance</param>
        /// <param name="customHeaders">Custom headers to be added as part of request headers</param>
        /// <param name="parameters">Parameters to include as part of query string</param>
        /// <returns>Returns the instance of type T if exists</returns>
        public Task<T> GetAsync<T>(
            string method,
            Action<HttpRequestHeaders> customHeaders = null,
            params KeyValuePair<string, object>[] parameters)
            where T : class
        {
            return RESTfulConsumer.GetAsync<T>(BaseUrl, method, IncludeAllHeaders(customHeaders), parameters);
        }

        /// <summary>
        /// Returns all instances of type T
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="method">Method to execute to retrieve instance</param>
        /// <param name="customHeaders">Custom headers to be added as part of request headers</param>
        /// <param name="parameters">Parameters to include as part of query string</param>
        /// <returns>Returns all the instance of type T</returns>
        public Task<IEnumerable<T>> GetAllAsync<T>(
            string method,
            Action<HttpRequestHeaders> customHeaders = null,
            params KeyValuePair<string, object>[] parameters)
            where T : class
        {
            return RESTfulConsumer.GetAllAsync<T>(BaseUrl, method, IncludeAllHeaders(customHeaders), parameters);
        }

        /// <summary>
        /// Sends a request of type T as a Post
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="objectToSend">Instance to send</param>
        /// <param name="method">Method to execute to send information</param>
        /// <param name="customHeaders">Custom headers to be added as part of request headers</param>
        /// <param name="parameters">Parameters to include as part of query string</param>
        /// <returns>Returns the HttpResponseMessage</returns>
        public Task<HttpResponseMessage> PostAsync<T>(
            T objectToSend,
            string method,
            Action<HttpRequestHeaders> customHeaders = null,
            params KeyValuePair<string, object>[] parameters)
            where T : class
        {
            return RESTfulConsumer.PostAsync(objectToSend, BaseUrl, method, IncludeAllHeaders(customHeaders), parameters);
        }

        /// <summary>
        /// Sends a request of type T as a Put
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="objectToSend">Instance to send</param>
        /// <param name="method">Method to execute to send information</param>
        /// <param name="customHeaders">Custom headers to be added as part of request headers</param>
        /// <param name="parameters">Parameters to include as part of query string</param>
        /// <returns>Returns the HttpResponseMessage</returns>
        public Task<HttpResponseMessage> PutAsync<T>(
            T objectToSend,
            string method,
            Action<HttpRequestHeaders> customHeaders = null,
            params KeyValuePair<string, object>[] parameters)
            where T : class
        {
            return RESTfulConsumer.PutAsync(objectToSend, BaseUrl, method, IncludeAllHeaders(customHeaders), parameters);
        }

        /// <summary>
        /// Sends a request as a Delete
        /// </summary>
        /// <param name="method">Method to execute to delete information</param>
        /// <param name="customHeaders">Custom headers to be added as part of request headers</param>
        /// <param name="parameters">Parameters to include as part of query string</param>
        /// <returns>Returns the HttpResponseMessage</returns>
        public Task<HttpResponseMessage> DeleteAsync(
            string method,
            Action<HttpRequestHeaders> customHeaders = null,
            params KeyValuePair<string, object>[] parameters)
        {
            return RESTfulConsumer.DeleteAsync(BaseUrl, method, IncludeAllHeaders(customHeaders), parameters);
        }

        /// <summary>
        /// Uploads a file
        /// </summary>
        /// <param name="file">File content</param>       
        /// <param name="method">Method to execute to send information</param>
        /// <param name="customHeaders">Custom headers to be added as part of request headers</param>
        /// <param name="parameters">Parameters to include as part of query string</param>
        /// <returns>Returns the HttpResponseMessage</returns>
        public Task<HttpResponseMessage> UploadAsync(
            byte[] file,
            string method,
            Action<HttpRequestHeaders> customHeaders = null,
            params KeyValuePair<string, object>[] parameters)
        {
            return RESTfulConsumer.UploadAsync(file, BaseUrl, method, IncludeAllHeaders(customHeaders), parameters);
        }

        private Action<HttpRequestHeaders> IncludeAllHeaders(Action<HttpRequestHeaders> customHeader = null)
        {
            var allHeaders = new List<Action<HttpRequestHeaders>>();

            if (!SecurityToken.IsNullOrEmpty())
                allHeaders.Add(RequestHeaderFactory.AddAuthorization(SecurityToken));

            if (customHeader.IsNotNull())
                allHeaders.Add(customHeader);

            if (allHeaders.IsNullOrEmpty())
                return null;

            return httpRequestHeaders =>
            {
                foreach (var header in allHeaders)
                    header(httpRequestHeaders);
            };
        }

        /// <summary>
        /// Sets the security token to be used in each request
        /// </summary>
        /// <param name="securityToken"></param>
        public void SetSecurityToken(string securityToken)
        {
            securityToken.NotNullOrEmpty(nameof(securityToken));

            SecurityToken = securityToken;
        }

        /// <summary>
        /// Releases all the managed resources
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;

            if (_selfthostedWebApi.IsNotNull())
                _selfthostedWebApi.Dispose();

            GC.SuppressFinalize(this);

            IsDisposed = true;
        }
    }
}
