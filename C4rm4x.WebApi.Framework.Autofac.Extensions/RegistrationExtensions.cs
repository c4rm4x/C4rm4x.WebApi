#region Using

using Autofac;
using Autofac.Extras.Multitenant;
using C4rm4x.Tools.Utilities;
using System;
using System.Linq;
using System.Reflection;

#endregion

namespace C4rm4x.WebApi.Framework.Autofac
{
    /// <summary>
    /// Utilities methods to auto-register all the API objects for both,
    /// Autofac container builder and Multi tenant container
    /// </summary>
    public static class RegistrationExtensions
    {
        private static void RegisterTypeByAttribute<TAttr>(
            this ContainerBuilder container,
            params Assembly[] assemblies)
            where TAttr : Attribute
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterAssemblyTypes(assemblies)
                .Where(t => t.GetCustomAttributes(false).Any(a => a.GetType() == typeof(TAttr)))
                .InstancePerRequest()
                .AsImplementedInterfaces();
        }       

        /// <summary>
        /// Registers all public classes decorated with attribute DomainService within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac container builder</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllDomainServices(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<DomainServiceAttribute>(assemblies);
        }

        /// <summary>
        /// Registers all public classes decorated with attribute Transformer within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac container builder</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllTransformers(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<TransformerAttribute>(assemblies);
        }

        /// <summary>
        /// Registers all public classes decorated with attribute Repository within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac container builder</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllRepositories(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<RepositoryAttribute>(assemblies);
        }

        /// <summary>
        /// Registers all public classes decorated with attribute ExecutionContextInitialiser within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac container builder</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllExecutionContextInitialisers(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<ExecutionContextInitialiserAttribute>(
                assemblies);
        }

        /// <summary>
        /// Registers all public classes decorated with attribute Validator within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac container builder</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllValidators(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<ValidatorAttribute>(assemblies);
        }

        /// <summary>
        /// Registers all public classes decorated with attribute RequestHandler within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac container builder</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllRequestHandlers(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<RequestHandlerAttribute>(assemblies);
        }

        /// <summary>
        /// Registers all public classes decorated with attribute EventHandler within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac container builder</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllEventHandlers(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<EventHandlerAttribute>(assemblies);
        }

        /// <summary>
        /// Registers all public classes decorated with attribute DataProvider within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac container builder</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllDataProviders(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<DataProviderAttribute>(assemblies);
        }

        /// <summary>
        /// Registers all public classes decorated with attributes DomainService, Transformer, Validator,
        /// Repository, RequestHandler, EventHandler, DataProvider and ExecutionContextInitialiser
        /// within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac container builder</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAll(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterAllDomainServices(assemblies);
            container.RegisterAllTransformers(assemblies);
            container.RegisterAllRepositories(assemblies);
            container.RegisterAllValidators(assemblies);
            container.RegisterAllRequestHandlers(assemblies);
            container.RegisterAllEventHandlers(assemblies);
            container.RegisterAllDataProviders(assemblies);
            container.RegisterAllExecutionContextInitialisers(assemblies);
        }

        private static void RegisterTypeByAttribute<TAttr>(
            this MultitenantContainer container,
            object tenantId,
            params Assembly[] assemblies)
            where TAttr : Attribute
        {
            container.NotNull(nameof(container));
            tenantId.NotNull(nameof(tenantId));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.ConfigureTenant(tenantId,
                b => b.RegisterTypeByAttribute<TAttr>(assemblies));
        }

        /// <summary>
        /// Registers tenant-specific all public classes decorated with attribute DomainService within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac multitenant container</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllDomainServices(
            this MultitenantContainer container,
            object tenantId,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            tenantId.NotNull(nameof(tenantId));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<DomainServiceAttribute>(tenantId, assemblies);
        }

        /// <summary>
        /// Registers tenant-specific all public classes decorated with attribute Transformer within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac multitenant container</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllTransformers(
            this MultitenantContainer container,
            object tenantId,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            tenantId.NotNull(nameof(tenantId));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<TransformerAttribute>(tenantId, assemblies);
        }

        /// <summary>
        /// Registers tenant-specific all public classes decorated with attribute Repository within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac multitenant container</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllRepositories(
            this MultitenantContainer container,
            object tenantId,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            tenantId.NotNull(nameof(tenantId));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<RepositoryAttribute>(tenantId, assemblies);
        }

        /// <summary>
        /// Registers tenant-specific all public classes decorated with attribute ExecutionContextInitialiser within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac multitenant container</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllExecutionContextInitialisers(
            this MultitenantContainer container,
            object tenantId,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            tenantId.NotNull(nameof(tenantId));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<ExecutionContextInitialiserAttribute>(
                tenantId, assemblies);
        }

        /// <summary>
        /// Registers tenant-specific all public classes decorated with attribute Validator within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac multitenant container</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllValidators(
            this MultitenantContainer container,
            object tenantId,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            tenantId.NotNull(nameof(tenantId));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<ValidatorAttribute>(tenantId, assemblies);
        }

        /// <summary>
        /// Registers tenant-specific all public classes decorated with attribute RequestHandler within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac multitenant container</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllRequestHandlers(
            this MultitenantContainer container,
            object tenantId,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            tenantId.NotNull(nameof(tenantId));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<RequestHandlerAttribute>(tenantId, assemblies);
        }

        /// <summary>
        /// Registers tenant-specific all public classes decorated with attribute EventHandler within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac multitenant container</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllEventHandlers(
            this MultitenantContainer container,
            object tenantId,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            tenantId.NotNull(nameof(tenantId));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<EventHandlerAttribute>(tenantId, assemblies);
        }

        /// <summary>
        /// Registers tenant-specific all public classes decorated with attribute DataProvider within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac multitenant container</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAllDataProviders(
            this MultitenantContainer container,
            object tenantId,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            tenantId.NotNull(nameof(tenantId));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterTypeByAttribute<DataProviderAttribute>(tenantId, assemblies);
        }

        /// <summary>
        /// Registers tenant-specific all public classes decorated with attributes DomainService, Transformer, Validator,
        /// Repository, RequestHandler, EventHandler, DataProvider and ExecutionContextInitialiser
        /// within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac multitenant container</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterAll(
            this MultitenantContainer container,
            object tenantId,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            tenantId.NotNull(nameof(tenantId));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterAllDomainServices(tenantId, assemblies);
            container.RegisterAllTransformers(tenantId, assemblies);
            container.RegisterAllRepositories(tenantId, assemblies);
            container.RegisterAllValidators(tenantId, assemblies);
            container.RegisterAllRequestHandlers(tenantId, assemblies);
            container.RegisterAllEventHandlers(tenantId, assemblies);
            container.RegisterAllDataProviders(tenantId, assemblies);
            container.RegisterAllExecutionContextInitialisers(tenantId, assemblies);
        }

        /// <summary>
        /// Registers all modules that inherit from VeModule within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac container builder</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterModules(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterAssemblyModules<ApiModule>(assemblies);
        }

        /// <summary>
        /// Registers all modules that inherit from MultitenantVeModule 
        /// within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac multitenant container</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterModules(
            this MultitenantContainer container,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            var moduleFinder = new ContainerBuilder();

            moduleFinder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(MultitenantApiModule).IsAssignableFrom(t))
                .As<IMultitenantApiModule>();

            using (var moduleContainer = moduleFinder.Build())
            {
                foreach (var module in moduleContainer.ResolveAll<IMultitenantApiModule>())
                    module.Load(container);
            }
        }

        /// <summary>
        /// Registers types that implement ApiController within all specified assemblies
        /// </summary>
        /// <param name="container">Autofac container builder</param>
        /// <param name="assemblies">List of assemblies</param>
        public static void RegisterWebApiControllers(
            this ContainerBuilder container,
            params Assembly[] assemblies)
        {
            container.NotNull(nameof(container));
            assemblies.NotNullOrEmpty(nameof(assemblies));

            container.RegisterAssemblyModules<MultitenantApiModule>(assemblies);
        }
    }
}
