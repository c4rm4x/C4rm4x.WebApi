namespace C4rm4x.WebApi.Framework.Runtime
{
    /// <summary>
    /// Interface that appends a new execution context extension based on type of request
    /// </summary>
    /// <typeparam name="TRequest">Type of request</typeparam>
    public interface IExecutionContextExtensionInitialiser<TRequest>
        where TRequest : ApiRequest
    {
        /// <summary>
        /// Appends a new execution context extension in the current execution context
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>A new instance of execution context extension for this specific type of request</returns>
        IExecutionContextExtension Append(TRequest request);
    }
}
