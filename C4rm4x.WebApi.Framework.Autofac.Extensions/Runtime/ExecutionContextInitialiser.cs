#region Using

using Autofac;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Runtime;
using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Framework.Autofac.Runtime
{
    /// <summary>
    /// Implementation of IExecutionContextInitialiser using the Autofac 
    /// container to retrieve the list of all ExecutionContextExtensionInitialisers
    /// registered in it
    /// </summary>
    public class ExecutionContextInitialiser :
        AbstractExecutionContextInitialiser, IExecutionContextInitialiser
    {
        private readonly IComponentContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executionContext">The current execution context</param>
        /// <param name="context">The Autofac context</param>
        public ExecutionContextInitialiser(
            IExecutionContext executionContext,
            IComponentContext context)
            : base(executionContext)
        {
            context.NotNull(nameof(context));

            _context = context;
        }

        /// <summary>
        /// Retrieves the list of all execution context extensions initialiser for the
        ///  specific type request
        /// </summary>
        /// <typeparam name="TRequest">Type of the request</typeparam>
        /// <returns>The list of all execution context extensions initialiser (if any) for the specific type of request</returns>
        protected override IEnumerable<IExecutionContextExtensionInitialiser<TRequest>>
            GetInitialisersPerRequest<TRequest>()
        {
            return _context
                .ResolveAll<IExecutionContextExtensionInitialiser<TRequest>>();
        }
    }
}
