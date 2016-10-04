#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Framework.Runtime
{
    #region Interface

    /// <summary>
    /// Service responsible to manage execution context extensions
    /// </summary>
    public interface IExecutionContext
    {
        /// <summary>
        /// Adds a new (or replaces if previous one exists) execution context 
        /// extension within the context
        /// </summary>
        /// <typeparam name="TExtension">Type of extension</typeparam>
        /// <param name="extension">The execution context extension to add</param>
        /// <exception cref="ArgumentException">Throws when a previous execution context extension of type TExtension already exists</exception>
        void AddExtension<TExtension>(
            TExtension extension)
            where TExtension : class, IExecutionContextExtension;

        /// <summary>
        /// Retrieves the execution context extension based on type
        /// </summary>
        /// <typeparam name="TExtension">Type of extension to retrieve</typeparam>
        /// <returns>The execution context extension whose type is TExtension</returns>
        /// <exception cref="ArgumentException">Throws when no execution context extension of type TExtension exists within</exception>
        TExtension GetExtension<TExtension>()
            where TExtension : class, IExecutionContextExtension;
    }

    #endregion

    /// <summary>
    /// Implementation of IExecutionContext
    /// </summary>
    [DomainService(typeof(IExecutionContext))]
    public class ExecutionContext : IExecutionContext
    {
        private readonly IDictionary<Type, IExecutionContextExtension> _extensions;

        /// <summary>
        /// Constructor
        /// </summary>
        public ExecutionContext()
        {
            _extensions = new Dictionary<Type, IExecutionContextExtension>();
        }

        /// <summary>
        /// Gets the collection of all extensions
        /// </summary>
        public IReadOnlyCollection<IExecutionContextExtension> Extensions
        {
            get { return _extensions.Values.ToList().AsReadOnly(); }
        }

        /// <summary>
        /// Adds a new (or replaces if previous one exists) execution context 
        /// extension within the context
        /// </summary>
        /// <typeparam name="TExtension">Type of extension</typeparam>
        /// <param name="extension">The execution context extension to add</param>
        /// <exception cref="ArgumentException">Throws when a previous execution context extension of type TExtension already exists</exception>
        public void AddExtension<TExtension>(
            TExtension extension)
            where TExtension : class, IExecutionContextExtension
        {
            _extensions.Add(extension.GetType(), extension);
        }

        /// <summary>
        /// Retrieves the execution context extension based on type
        /// </summary>
        /// <typeparam name="TExtension">Type of extension to retrieve</typeparam>
        /// <returns>The execution context extension whose type is TExtension</returns>
        /// <exception cref="ArgumentException">Throws when no execution context extension of type TExtension exists within</exception>
        public TExtension GetExtension<TExtension>()
            where TExtension : class, IExecutionContextExtension
        {
            if (!_extensions.ContainsKey(typeof(TExtension)))
                throw new ArgumentException(
                    "There is not extension of type {0}"
                        .AsFormat(typeof(TExtension).Name));

            return _extensions[typeof(TExtension)] as TExtension;
        }
    }
}
