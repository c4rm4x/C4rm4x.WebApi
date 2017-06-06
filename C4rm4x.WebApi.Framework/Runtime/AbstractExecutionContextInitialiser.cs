#region Using

using C4rm4x.Tools.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Runtime
{
    /// <summary>
    /// Baes implementation of IExecutionContextInitialiser
    /// </summary>
    public abstract class AbstractExecutionContextInitialiser :
        IExecutionContextInitialiser
    {
        private readonly IExecutionContext _executionContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executionContext">The current execution context</param>
        public AbstractExecutionContextInitialiser(
            IExecutionContext executionContext)
        {
            executionContext.NotNull(nameof(executionContext));

            _executionContext = executionContext;
        }

        /// <summary>
        /// Initialises the current execution context per request
        /// </summary>
        /// <typeparam name="TRequest">Type of request</typeparam>
        /// <param name="request">The request</param>
        public async Task PerRequestAsync<TRequest>(TRequest request)
            where TRequest : ApiRequest
        {
            await Task.WhenAll(
                GetInitialisersPerRequest<TRequest>().Select(
                    initialiser => initialiser.AddAsync(_executionContext, request)));
        }

        /// <summary>
        /// Retrieves the list of all execution context extensions initialiser for the specific type request
        /// </summary>
        /// <typeparam name="TRequest">Type of the request</typeparam>
        /// <returns>The list of all execution context extensions initialiser (if any) for the specific type of request</returns>
        protected abstract IEnumerable<IExecutionContextExtensionInitialiser<TRequest>>
            GetInitialisersPerRequest<TRequest>()
            where TRequest : ApiRequest;
    }
}
