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
    public abstract class AbstractQueryableTransformer<S, D>
        : IQueryableTransformer<S, D>
    {
        /// <summary>
        /// Applies transformation logic from source to destination
        /// </summary>
        /// <param name="source">Source</param>
        /// <returns>Destination</returns>
        public abstract D Transform(S source);

        /// <summary>
        /// Applies transformation logic from source to destination
        /// </summary>
        /// <param name="sources">Sources</param>        
        /// <returns>Destinations</returns>
        public IEnumerable<D> Transform(IQueryable<S> sources)
        {
            return sources.Select(Transform);
        }
    }
}
