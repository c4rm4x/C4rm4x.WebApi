#region Using

using Autofac;
using C4rm4x.Tools.Utilities;
using System.Reflection;
using AutofacModule = Autofac.Module;

#endregion

namespace C4rm4x.WebApi.Framework.Autofac
{
    /// <summary>
    /// Provides a user-friendly way to implement Autofac.Core.IModule via Autofac.ContainerBuilder
    /// </summary>
    public abstract class ApiModule : AutofacModule
    {
        /// <summary>
        /// Adds registration to the Autofac Container Builder
        /// </summary>
        /// <param name="builder">The Autofa container builder</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.NotNull(nameof(builder));

            base.Load(builder);

            RegisterDependencies(builder);
        }

        /// <summary>
        /// Registers dependencies to the Autofac container builder
        /// </summary>
        /// <param name="builder">The container builder</param>
        /// <remarks>
        /// Default implementation registers all API objects
        /// </remarks>
        protected virtual void RegisterDependencies(
            ContainerBuilder builder)
        {
            builder.RegisterAll(this.ThisAssembly);
        }

        /// <summary>
        /// Needs to be overriden because base property throws InvalidOperationException
        /// when base type is not Module
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
