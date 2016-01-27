namespace C4rm4x.WebApi.Framework.RequestHandling
{
    /// <summary>
    /// Service responsible to retrieve the instance that implmement
    /// IHandler for the specified type of request
    /// </summary>
    public interface IHandlerFactory
    {
        /// <summary>
        /// Gets the handler for the specified request type.
        /// </summary>
        IHandler<TRequest> GetHandler<TRequest>()
            where TRequest : ApiRequest;
    }
}
