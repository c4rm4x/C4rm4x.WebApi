#region Using

using C4rm4x.Tools.Utilities;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Runtime
{
    /// <summary>
    /// Execution context extensions
    /// </summary>
    public static class IExecutionContextExtensions
    {
        /// <summary>
        /// Adds a new (or replaces if previous one exists) execution context
        /// extension within the context
        /// </summary>
        /// <typeparam name="TExtension">Type of the extension to add</typeparam>
        /// <param name="context">The execution context</param>
        /// <param name="extension">The execution context to add</param>
        public static void Add<TExtension>(
            this IExecutionContext context,
            TExtension extension)
        {
            context.NotNull(nameof(context));

            context.Add(typeof(TExtension).FullName, extension);
        }

        /// <summary>
        /// Retrieves the execution context extension based on type full name
        /// </summary>
        /// <typeparam name="TExtension">Type of the extension to retrieve</typeparam>
        /// <param name="context">The execution context</param>
        /// <returns>The execution context extension whose identifier is type full name</returns>
        /// <exception cref="ArgumentException">Throws when no execution context extension of given type exists within</exception>
        public static TExtension Get<TExtension>(
            this IExecutionContext context)
        {
            return context.Get<TExtension>(typeof(TExtension).FullName);
        }

        /// <summary>
        /// Retrieve the execution context extension based on key
        /// </summary>
        /// <typeparam name="TExtension">Type of the extension to retrieve</typeparam>
        /// <param name="context">The execution context</param>
        /// <param name="key">The key</param>
        /// <returns>The execution context extension whose identifier is given key</returns>
        /// <exception cref="ArgumentException">Throws when no execution context extension of given key exists within</exception>
        public static TExtension Get<TExtension>(
            this IExecutionContext context,
            string key)
        {
            context.NotNull(nameof(context));
            key.NotNullOrEmpty(nameof(key));

            return (TExtension)context.Get(key);
        }
    }
}
