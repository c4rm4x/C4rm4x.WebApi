#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Collections.Concurrent;

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
        /// <param name="key">The identifier</param>
        /// <param name="extension">The execution context to add</param>
        void Add(string key, object extension);

        /// <summary>
        /// Retrieves the execution context extension based on key
        /// </summary>
        /// <param name="key">The execution context extension whose identifier is key</param>
        /// <returns>The execution context extension whose identifier is key</returns>
        /// <exception cref="ArgumentException">Throws when no execution context extension of given key exists within</exception>
        object Get(string key);
    }

    #endregion

    /// <summary>
    /// Implementation of IExecutionContext
    /// </summary>   
    public class ExecutionContext : IExecutionContext
    {
        private readonly ConcurrentDictionary<string, object> _extensions;

        /// <summary>
        /// Constructor
        /// </summary>
        public ExecutionContext()
        {
            _extensions = new ConcurrentDictionary<string, object>();
        }

        /// <summary>
        /// Adds a new (or replaces if previous one exists) execution context
        /// extension within the context
        /// </summary>
        /// <param name="key">The identifier</param>
        /// <param name="extension">The execution context to add</param>
        public void Add(string key, object extension)            
        {
            key.NotNullOrEmpty(nameof(key));

            _extensions.AddOrUpdate(key, extension, (id, oldValue) => extension);
        }

        /// <summary>
        /// Retrieves the execution context extension based on key
        /// </summary>
        /// <param name="key">The execution context extension whose identifier is key</param>
        /// <returns>The execution context extension whose identifier is key</returns>
        /// <exception cref="ArgumentException">Throws when no execution context extension of given key exists within</exception>
        public object Get(string key)            
        {
            key.NotNullOrEmpty(nameof(key));

            if (!_extensions.ContainsKey(key))
                throw new ArgumentException(
                    "There is not extension of type {0}".AsFormat(key));

            object value;
            return _extensions.TryGetValue(key, out value) ? value : null;
        }
    }
}
