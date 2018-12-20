#region Using

using C4rm4x.Tools.TestUtilities.Bdd;
using C4rm4x.Tools.TestUtilities.EF;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.TestUtilities.Acceptance.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using TestContext = C4rm4x.Tools.TestUtilities.TestContext;

#endregion

namespace C4rm4x.WebApi.TestUtilities.Acceptance
{
    /// <summary>
    /// Base test class for acceptance tests
    /// </summary>
    [TestClass]
    public abstract class AcceptanceFixture
    {
        private Container _container;
        private Scope _scope;
        private GivenWhenThen _givenWhenThen;

        private readonly ICollection<Action<HttpConfiguration>> _middleware =
            new HashSet<Action<HttpConfiguration>>();

        /// <summary>
        /// The test context
        /// </summary>
        protected TestContext Context { get; private set; }
            = new TestContext();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configurator">Http configurator</param>
        public AcceptanceFixture(Action<HttpConfiguration> configurator)
        {
            configurator.NotNull(nameof(configurator));

            _middleware.Add(configurator);
        }

        /// <summary>
        /// Initialises the test class
        /// </summary>
        [TestInitialize]
        public virtual void Setup()
        {
            SetupContainer();
            SetupHttpServer();
        }

        private void SetupContainer()
        {
            _container = new Container();

            RegisterDependencies(_container, new LifetimeScopeLifestyle()); // Registers the rest

            _container.Verify();

            _scope = _container.BeginLifetimeScope(); // Starts container life time scope
        }

        private void SetupHttpServer()
        {
            HttpServer.Configure(_middleware.ToArray());
        }

        /// <summary>
        /// Finalises the test class
        /// </summary>
        [TestCleanup]
        public virtual void Cleanup()
        {
            _givenWhenThen = null;

            _scope.Dispose(); // Enforce to dispose all the components

            Context.Cleanup();
        }

        /// <summary>
        /// Use the given middleware
        /// </summary>
        /// <param name="middleware">The middleware</param>
        protected void WithMiddleware(Action<HttpConfiguration> middleware)
        {
            middleware.NotNull(nameof(middleware));

            _middleware.Add(middleware);
        }

        /// <summary>
        /// Registers all the dependencies for this acceptance test
        /// </summary>
        /// <param name="container">The container</param>
        /// <param name="lifeStyle">The life style that specifies how the return instance will be cached</param>
        protected virtual void RegisterDependencies(
            Container container,
            Lifestyle lifeStyle)
        {
            container.Register<InMemoryHttpServer>(lifeStyle);
        }

        /// <summary>
        /// Returns the Http response from a Get request
        /// </summary>
        /// <param name="method">Method to execute to retrieve instance</param>
        /// <param name="customHeaders">Custom headers to be added as part of request headers</param>
        /// <param name="parameters">Parameters to include as part of query string</param>
        /// <returns>Returns the HttpResponseMessage</returns>
        protected Task<HttpResponseMessage> GetAsync(
            string method,
            Action<HttpRequestHeaders> customHeaders = null,
            params KeyValuePair<string, object>[] parameters)
        {
            return HttpServer.GetAsync(method, customHeaders, parameters);
        }

        /// <summary>
        /// Returns an instance of type T
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="method">Method to execute to retrieve instance</param>
        /// <param name="customHeaders">Custom headers to be added as part of request headers</param>
        /// <param name="parameters">Parameters to include as part of query string</param>
        /// <returns>Returns the instance of type T if exists</returns>
        protected Task<T> GetAsync<T>(
            string method,
            Action<HttpRequestHeaders> customHeaders = null,
            params KeyValuePair<string, object>[] parameters)
            where T : class
        {
            return HttpServer.GetAsync<T>(method, customHeaders, parameters);
        }

        /// <summary>
        /// Returns all instances of type T
        /// </summary>
        /// <typeparam name="T">The type of the instance</typeparam>
        /// <param name="method">Method to execute to retrieve instance</param>
        /// <param name="customHeaders">Custom headers to be added as part of request headers</param>
        /// <param name="parameters">Parameters to include as part of query string</param>
        /// <returns>Returns all the instance of type T</returns>
        protected Task<IEnumerable<T>> GetAllAsync<T>(
            string method,
            Action<HttpRequestHeaders> customHeaders = null,
            params KeyValuePair<string, object>[] parameters)
            where T : class
        {
            return HttpServer.GetAllAsync<T>(method, customHeaders, parameters);
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
        protected Task<HttpResponseMessage> PostAsync<T>(
            T objectToSend,
            string method,
            Action<HttpRequestHeaders> customHeaders = null,
            params KeyValuePair<string, object>[] parameters)
            where T : class
        {
            return HttpServer.PostAsync(objectToSend, method, customHeaders, parameters);
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
        protected Task<HttpResponseMessage> PutAsync<T>(
            T objectToSend,
            string method,
            Action<HttpRequestHeaders> customHeaders = null,
            params KeyValuePair<string, object>[] parameters)
            where T : class
        {
            return HttpServer.PutAsync(objectToSend, method, customHeaders, parameters);
        }

        /// <summary>
        /// Sends a request as a Delete
        /// </summary>
        /// <param name="method">Method to execute to delete the information</param>
        /// <param name="customHeaders">Custom headers to be added as part of request headers</param>
        /// <param name="parameters">Parameters to include as part of query string</param>
        /// <returns>Returns the HttpResponseMessage</returns>
        protected Task<HttpResponseMessage> DeleteAsync(
            string method,
            Action<HttpRequestHeaders> customHeaders = null,
            params KeyValuePair<string, object>[] parameters)
        {
            return HttpServer.DeleteAsync(method, customHeaders, parameters);
        }

        /// <summary>
        /// Uploads a file
        /// </summary>
        /// <param name="file">File content</param>
        /// <param name="method">Method to execute to send information</param>
        /// <param name="customHeaders">Custom headers to be added as part of request headers</param>
        /// <param name="parameters">Parameters to include as part of query string</param>
        /// <returns>Returns the HttpResponseMessage</returns>
        protected Task<HttpResponseMessage> UploadAsync(
            byte[] file,
            string method,
            Action<HttpRequestHeaders> customHeaders = null,
            params KeyValuePair<string, object>[] parameters)
        {
            return HttpServer.UploadAsync(file, method, customHeaders, parameters);
        }

        /// <summary>
        /// Starts a new test plan with the initial given step
        /// </summary>
        /// <param name="step">The initial given step</param>
        protected IGivenDefinition Given(GivenHandler step)
        {
            _givenWhenThen = GivenWhenThen.StartWith(step);

            return _givenWhenThen;
        }

        /// <summary>
        /// Starts a new test plan with the initial when step
        /// </summary>
        /// <param name="step">Then initial when step</param>
        protected IWhenDefinition When(WhenHandler step)
        {
            _givenWhenThen = GivenWhenThen.StartWith(step);

            return _givenWhenThen;
        }

        /// <summary>
        /// Given step to get a basic token
        /// </summary>
        protected void UserIsLoggedIn()
        {
            UserIsLoggedInAs();
        }

        /// <summary>
        /// Given step to get a token with the given role/permissions
        /// </summary>
        /// <param name="role">The role</param>
        /// <param name="permissions">The set of permissions</param>
        protected void UserIsLoggedInAs(
            string role = null,
            params KeyValuePair<string, object>[] permissions)
        {
            var userIsLoggedInAsStep = new UserIsLoggedInAsStep(HttpServer);

            userIsLoggedInAsStep.TokenFor(role, permissions);
        }

        /// <summary>
        /// Given step to get an authorization token with the given identifier/secret
        /// </summary>
        /// <param name="identifier">The identifier</param>
        /// <param name="sharedSecret">The secret (as base-64)</param>
        protected void ClientIsIdentifiedWith(
            string identifier,
            string sharedSecret)
        {
            var clientIsIdentifierWithStep = new ClientIsIdentifiedWithStep(HttpServer);

            clientIsIdentifierWithStep.AuthorizationFor(identifier, sharedSecret);
        }

        /// <summary>
        /// Gets an instance of the specified type
        /// </summary>
        /// <typeparam name="TService">Type of instance</typeparam>
        /// <returns>An instance of specified type</returns>
        /// <exception cref="SimpleInjector.ActivationException">Thrown when there are errors resolving the service instance</exception>
        protected TService GetInstance<TService>()
            where TService : class
        {
            return _container.GetInstance<TService>();
        }

        private InMemoryHttpServer HttpServer
        {
            get { return GetInstance<InMemoryHttpServer>(); }
        }
    }

    /// <summary>
    /// Base test class for acceptance tests using Entity Framework for access to the database
    /// </summary>
    /// <typeparam name="TContext">Type of DbContext</typeparam>
    [TestClass]
    public abstract class AcceptanceFixture<TContext> :
        AcceptanceFixture
        where TContext : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configurator">Http configurator</param>
        public AcceptanceFixture(Action<HttpConfiguration> configurator)
            : base(configurator)
        { }

        /// <summary>
        /// Finalises the test class
        /// </summary>
        [TestCleanup]
        public override void Cleanup()
        {
            EntityManager.Restore();

            base.Cleanup();
        }

        /// <summary>
        /// Registers all the dependencies for this acceptance test
        /// </summary>
        /// <param name="container">The container</param>
        /// <param name="lifeStyle">The life style that specifies how the return instance will be cached</param>
        protected override void RegisterDependencies(
            Container container,
            Lifestyle lifeStyle)
        {
            container.Register<EntityManager<TContext>>(lifeStyle);
            container.Register<TContext>(lifeStyle);

            base.RegisterDependencies(container, lifeStyle);
        }

        /// <summary>
        /// Adds a new entity to the db context
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        /// <param name="entity">Entity to add</param>
        /// <param name="saveContext">Indicates whether the entity must be saved in the context</param>
        protected void AddEntityToContext<TEntity>(
            TEntity entity,
            bool saveContext = false)
            where TEntity : class
        {
            EntityManager.Add(entity);

            if (saveContext)
                SaveContext();
        }

        /// <summary>
        /// Saves all the entities within the context into the database
        /// </summary>
        protected void SaveContext()
        {
            EntityManager.SaveAllChanges();
        }

        private EntityManager<TContext> EntityManager
        {
            get { return GetInstance<EntityManager<TContext>>(); }
        }
    }
}
