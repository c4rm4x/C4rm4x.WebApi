#region Using

using C4rm4x.Tools.Utilities;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.RequestHandling
{
    /// <summary>
    /// Base Api Controller for all the projects
    /// </summary>
    public abstract class AbstractApiController : ApiController
    {
        private readonly IHandlerShell _shell;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shell">The handler shell</param>
        public AbstractApiController(IHandlerShell shell)
        {
            shell.NotNull(nameof(shell));

            _shell = shell;
        }

        /// <summary>
        /// Handles an specific request based on type
        /// </summary>
        /// <typeparam name="TRequest">The type of the request</typeparam>
        /// <param name="request">The request to be handled</param>
        /// <returns>Returns an instance of IHttpActionResult</returns>
        protected async Task<IHttpActionResult> HandleAsync<TRequest>(
            TRequest request)
            where TRequest : ApiRequest
        {
            return await _shell.HandleAsync(request);            
        }        
    }
}
