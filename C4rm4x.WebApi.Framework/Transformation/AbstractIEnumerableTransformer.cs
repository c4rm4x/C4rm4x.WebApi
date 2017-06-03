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
    public abstract class AbstractEnumerableTransformer<S, D>
        : IEnumerableTransformer<S, D>
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
        public IEnumerable<D> Transform(IEnumerable<S> sources)
        {
            return sources.Select(Transform);
        }
    }
}
