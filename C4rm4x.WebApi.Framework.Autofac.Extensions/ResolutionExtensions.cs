#region Using

using Autofac;
using C4rm4x.Tools.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Framework.Autofac
{
    /// <summary>
    /// Extesions for resolution using Autofac container
    /// </summary>
    internal static class ResolutionExtensions
    {
        /// <summary>
        /// Retrieves a list of services of type TService from the context
        /// </summary>
        /// <typeparam name="TService">Type of service</typeparam>
        /// <param name="context">The context from which to resolve the list of services</param>
        /// <returns>The list of component instances that provide the service</returns>
        public static IEnumerable<TService> ResolveAll<TService>(
            this IComponentContext context)
            where TService : class
        {
            context.NotNull(nameof(context));

            return context.ResolveOptional<IEnumerable<TService>>();
        }

        /// <summary>
        /// Retrieves a list of services of a given type from the context
        /// </summary>
        /// <param name="context">The context from which to resolve the list of services</param>
        /// <param name="type">Type of service</param>
        /// <returns>The list of component instances that provide the service</returns>
        public static IEnumerable ResolveAll(
            this IComponentContext context,
            Type type)
        {
            context.NotNull("container");
            type.NotNull("type");

            return (IEnumerable)context.ResolveOptional(typeof(IEnumerable<>).MakeGenericType(type));
        }
    }
}
