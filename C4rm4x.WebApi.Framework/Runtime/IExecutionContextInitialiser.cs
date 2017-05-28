#region Using

using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Runtime
{
    /// <summary>
    /// Interface responsible to initialise the current execution context
    /// </summary>
    public interface IExecutionContextInitialiser
    {
        /// <summary>
        /// Initialises the current execution context per request
        /// </summary>
        /// <typeparam name="TRequest">Type of request</typeparam>
        /// <param name="request">The request</param>
        /// <returns>The task</returns>
        Task PerRequestAsync<TRequest>(TRequest request)
            where TRequest : ApiRequest;
    }
}
