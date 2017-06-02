namespace C4rm4x.WebApi.Framework.Transformation
{
    /// <summary>
    /// Service responsible to transform from objects of type S to objects of type D
    /// </summary>
    /// <typeparam name="S">Type of source</typeparam>
    /// <typeparam name="D">Type of destination</typeparam>
    public interface ITransformer<S, D>
    {
        /// <summary>
        /// Applies transformation logic from source to destination
        /// </summary>
        /// <param name="source">Source</param>
        /// <returns>Destination</returns>
        D Transform(S source);
    }

    /// <summary>
    /// Service responsible to transform from objects of type S to objects of type D
    /// using a context of type C
    /// </summary>
    /// <typeparam name="S">Type of source</typeparam>
    /// <typeparam name="D">Type of destination</typeparam>
    /// <typeparam name="C">Type of context</typeparam>
    public interface ITransformer<S, D, C>
    {
        /// <summary>
        /// Applies transfromation logic from source to destination using an auxiliar element to pass extra pieces of information needed outside source
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="context">Context</param>
        /// <returns>Destination</returns>
        D Transform(S source, C context);
    }
}
