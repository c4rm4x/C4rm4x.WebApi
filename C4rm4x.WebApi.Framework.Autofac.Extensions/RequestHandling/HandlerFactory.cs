#region Using

using Autofac;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.RequestHandling;

#endregion

namespace C4rm4x.WebApi.Framework.Autofac.Extensions.RequestHandling
{
    /// <summary>
    /// Implementation of IHandlerFactory using Autofac container
    /// </summary>
    public class HandlerFactory : IHandlerFactory
    {
        private readonly IComponentContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The Autofac context</param>
        public HandlerFactory(IComponentContext context)
        {
            context.NotNull(nameof(context));

            _context = context;
        }

        /// <summary>
        /// Retrieves the request handler for the specified type
        /// </summary>
        /// <typeparam name="TRequest">Type of the request</typeparam>
        /// <returns>The instance that implement IHandler for ths specified type</returns>
        public IHandler<TRequest> GetHandler<TRequest>()
            where TRequest : ApiRequest
        {
            return _context.Resolve<IHandler<TRequest>>();
        }
    }
}
