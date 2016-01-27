#region Using

using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Framework.Transformation
{
    /// <summary>
    /// Transform collection of objects to the specified destination
    /// </summary>
    /// <typeparam name="S">Type of the source objects</typeparam>
    /// <typeparam name="D">Type of the destination objects</typeparam>
    public interface ICollectionTransformer<S, D> : ITransformer<S, D>
    {
        /// <summary>
        /// Applies transformatin logic from a collection of source to a collection of destination
        /// </summary>
        /// <param name="sources">Collection of sources</param>
        /// <param name="destinations">Collection of destinations</param>
        void Transform(ICollection<S> sources, ICollection<D> destinations);
    }

    /// <summary>
    /// Transform collection of objects to the specified destination using a context
    /// </summary>
    /// <typeparam name="S">Type of the source objects</typeparam>
    /// <typeparam name="D">Type of the destination objects</typeparam>
    /// <typeparam name="C">Type of the context</typeparam>
    public interface ICollectionTransformer<S, D, C> : ITransformer<S, D, C>
    {
        /// <summary>
        /// Applies transfromation logic from a collection of source to a collection of destination 
        /// using an auxiliar element to pass extra pieces of information needed outside source
        /// </summary>
        /// <param name="sources">Collection of sources</param>
        /// <param name="destinations">Collection of destinations</param>
        /// <param name="context">Context</param>
        void Transform(ICollection<S> sources, ICollection<D> destinations, C context);
    }
}
