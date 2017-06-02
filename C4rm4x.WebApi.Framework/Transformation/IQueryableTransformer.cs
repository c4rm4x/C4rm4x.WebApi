#region Using

using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Framework.Transformation
{
    /// <summary>
    /// Transform queryable of objects to the specified destination
    /// </summary>
    /// <typeparam name="S">Type of the source objects</typeparam>
    /// <typeparam name="D">Type of the destination objects</typeparam>
    public interface IQueryableTransformer<S, D> : ITransformer<S, D>
    {
        /// <summary>
        /// Applies transformatin logic from a collection of source to a collection of destination
        /// </summary>
        /// <param name="sources">IQueryable of sources</param>
        /// <returns>Enumerable of destinations</returns>
        IEnumerable<D> Transform(IQueryable<S> sources);
    }

    /// <summary>
    /// Transform queryable of objects to the specified destination using a context
    /// </summary>
    /// <typeparam name="S">Type of the source objects</typeparam>
    /// <typeparam name="D">Type of the destination objects</typeparam>
    /// <typeparam name="C">Type of the context</typeparam>
    public interface IQueryableTransformer<S, D, C> : ITransformer<S, D, C>
    {
        /// <summary>
        /// Applies transfromation logic from a collection of source to a collection of destination 
        /// using an auxiliar element to pass extra pieces of information needed outside source
        /// </summary>
        /// <param name="sources">IQueryable of sources</param>
        /// <param name="context">Context</param>
        /// <returns>Enumerable of destinations</returns>
        IEnumerable<D> Transform(IQueryable<S> sources, C context);
    }
}
