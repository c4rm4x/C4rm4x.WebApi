#region Using

using C4rm4x.Tools.Utilities;
using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Framework.Transformation
{
    /// <summary>
    /// Transform collection of objects to the specified destination
    /// </summary>
    /// <typeparam name="S">Type of the source objects</typeparam>
    /// <typeparam name="D">Type of the destination objects</typeparam>
    public abstract class AbstractCollectionTransformer<S, D> :
        ICollectionTransformer<S, D>
        where D : class, new()
    {
        /// <summary>
        /// Applies transformation logic from source to destination
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        public abstract void Transform(
            S source, 
            D destination);

        /// <summary>
        /// Applies transformatin logic from a collection of source to a collection of destination
        /// </summary>
        /// <param name="sources">Collection of sources</param>
        /// <param name="destinations">Collection of destinations</param>
        public void Transform(
            ICollection<S> sources, 
            ICollection<D> destinations)
        {
            if (sources.IsNullOrEmpty()) return;

            foreach (var source in sources)
            {
                var destination = new D();
                Transform(source, destination);
                destinations.Add(destination);
            }
        }
    }
}
