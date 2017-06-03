#region Using

using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Framework.Transformation
{
    /// <summary>
    /// Transform IEnumerable of objects to the specified destination
    /// </summary>
    /// <typeparam name="S">Type of the source objects</typeparam>
    /// <typeparam name="D">Type of the destination objects</typeparam>
    public interface IEnumerableTransformer<S, D> : ITransformer<S, D>
    {
        /// <summary>
        /// Applies transformatin logic from a collection of source to a collection of destination
        /// </summary>
        /// <param name="sources">IEnumerable of sources</param>
        /// <returns>IEnumerable of destinations</returns>
        IEnumerable<D> Transform(IEnumerable<S> sources);
    }

    /// <summary>
    /// Transform IEnumerable of objects to the specified destination using a context
    /// </summary>
    /// <typeparam name="S">Type of the source objects</typeparam>
    /// <typeparam name="D">Type of the destination objects</typeparam>
    /// <typeparam name="C">Type of the context</typeparam>
    public interface IEnumerableTransformer<S, D, C> : ITransformer<S, D, C>
    {
        /// <summary>
        /// Applies transfromation logic from a collection of source to a collection of destination 
        /// using an auxiliar element to pass extra pieces of information needed outside source
        /// </summary>
        /// <param name="sources">IEnumerable of sources</param>
        /// <param name="context">Context</param>
        /// <returns>Enumerable of destinations</returns>
        IEnumerable<D> Transform(IEnumerable<S> sources, C context);
    }
}
