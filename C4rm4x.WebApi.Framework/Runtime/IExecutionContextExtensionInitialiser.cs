#region Using

using System.Threading.Tasks;

#endregion

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
        /// Adds a new execution context extension in the given execution context
        /// </summary>
        /// <param name="context">The execution context</param>
        /// <param name="request">The request</param>
        /// <returns>A new instance of execution context extension for this specific type of request</returns>
        Task AddAsync(IExecutionContext context, TRequest request);
    }
}
