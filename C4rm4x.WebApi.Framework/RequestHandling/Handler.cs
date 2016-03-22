#region Using

using C4rm4x.Tools.Utilities;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.RequestHandling
{
    #region Interface

    /// <summary>
    /// Handles an specific type of request
    /// </summary>
    /// <typeparam name="TRequest">Type of request</typeparam>
    public interface IHandler<TRequest>
        where TRequest : ApiRequest
    {
        /// <summary>
        /// Handles a request of the specified type
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>Returns an instance of IHttpActionResult</returns>
        IHttpActionResult Handle(TRequest request);
    }

    #endregion

    /// <summary>
    /// Base class that implements IHandler for an specific type of request
    /// </summary>
    /// <typeparam name="TRequest">Type of request</typeparam>
    /// <typeparam name="TResponse">Type of response</typeparam>
    public abstract class Handler<TRequest, TResponse> : IHandler<TRequest>
        where TRequest : ApiRequest
        where TResponse : ApiResponse
    {
        private readonly IHandlerShell _shell;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shell">The handler shell</param>
        public Handler(IHandlerShell shell)
        {
            shell.NotNull(nameof(shell));

            (_shell = shell).PolicyName = PolicyName;
        }

        /// <summary>
        /// Handles a request of the specified type
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>Returns an instance of IHttpActionResult</returns>
        public IHttpActionResult Handle(TRequest request)
        {
            return _shell.Process<TRequest, TResponse>(request, Process);
        }

        /// <summary>
        /// Processes the request and returns the value when everything goes Ok
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <returns>ApiResponse to return as part of the Ok response</returns>
        protected abstract TResponse Process(TRequest request);

        /// <summary>
        /// Policy name which handles exceptions for each handler
        /// </summary>
        /// <remarks>
        /// This value is meant to be the same for all handlers (but if for some reason needs to have a different value....)
        /// </remarks>
        protected virtual string PolicyName
        {
            get { return "default"; }
        }
    }
}
