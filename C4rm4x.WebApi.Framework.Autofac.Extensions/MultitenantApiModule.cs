#region Using

using Autofac;
using Autofac.Extras.Multitenant;
using Autofac.Integration.WebApi;
using C4rm4x.Tools.Utilities;
using System.Reflection;
using AutofacModule = Autofac.Module;

#endregion

namespace C4rm4x.WebApi.Framework.Autofac
{
    #region Interface

    /// <summary>
    /// Provides a user-friendly way to implement Autofac.Core.IModule via Autofac.MultitenantContainer.
    /// </summary>
    interface IMultitenantApiModule
    {
        /// <summary>
        /// Adds tenant-specific registrations to the container 
        /// </summary>
        /// <param name="container">Autofac multitenant container</param>
        void Load(MultitenantContainer container);
    }

    #endregion

    /// <summary>
    /// Base class that implements both, IModule and IMultitenantVeModule
    /// </summary>
    public abstract class MultitenantApiModule :
        AutofacModule, IMultitenantApiModule
    {
        /// <summary>
        /// Adds registration to the Autofact container builder
        /// </summary>
        /// <param name="builder">The Autofac container builder</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.NotNull(nameof(builder));

            base.Load(builder);

            builder.RegisterApiControllers(this.ThisAssembly);
        }

        /// <summary>
        /// Adds tenant-specific registrations to the container 
        /// </summary>
        /// <param name="container">Autofac multitenant container</param>
        public void Load(MultitenantContainer container)
        {
            container.NotNull(nameof(container));

            RegisterDependencies(container);
        }

        /// <summary>
        /// Register dependencies to the multitenant container
        /// </summary>
        /// <param name="container">The Autofac multi tenant container</param>
        /// <remarks>
        /// Default implementation register all API objects
        /// </remarks>
        protected virtual void RegisterDependencies(
            MultitenantContainer container)
        {
            container.RegisterAll(TenantId, this.ThisAssembly);
        }

        /// <summary>
        /// The tenant identifier associated to this module
        /// </summary>
        protected abstract object TenantId { get; }

        /// <summary>
        /// Gets the assembly in which the concrete module type is located
        /// </summary>
        protected override Assembly ThisAssembly
        {
            get
            {
                return GetType().Assembly;
            }
        }
    }
}
