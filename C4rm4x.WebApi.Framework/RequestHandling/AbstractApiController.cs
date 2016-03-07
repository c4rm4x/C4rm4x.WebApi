#region Using

using C4rm4x.Tools.Utilities;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.RequestHandling
{
    /// <summary>
    /// Base Api Controller for all the projects
    /// </summary>
    public abstract class AbstractApiController : ApiController
    {
        private readonly IHandlerFactory _handlerFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="handlerFactory">The request handler factory</param>
        public AbstractApiController(
            IHandlerFactory handlerFactory)
        {
            handlerFactory.NotNull(nameof(handlerFactory));
        }

        /// <summary>
        /// Handles an specific request based on type
        /// </summary>
        /// <typeparam name="TRequest">The type of the request</typeparam>
        /// <param name="request">The request to be handled</param>
        /// <returns>Returns an instance of IHttpActionResult</returns>
        protected IHttpActionResult Handle<TRequest>(TRequest request)
            where TRequest : ApiRequest
        {
            return _handlerFactory
                .GetHandler<TRequest>()
                .Handle(request);
        }
    }
}
