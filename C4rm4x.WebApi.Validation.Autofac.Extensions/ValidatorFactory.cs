#region Using

using Autofac;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Validation;
using System;

#endregion

namespace C4rm4x.WebApi.Validation.Autofac
{
    /// <summary>
    /// Implementation of IValidatorFactory using Autofac container
    /// </summary>
    public class ValidatorFactory :
        AbstractValidatorFactory, IValidatorFactory
    {
        private readonly IComponentContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The Autofac context</param>
        public ValidatorFactory(IComponentContext context)
        {
            context.NotNull(nameof(context));

            _context = context;
        }

        /// <summary>
        /// Returns the instance of a validator for the specified type
        /// </summary>
        /// <param name="type">Type of validator</param>
        /// <returns>The instance of validator of specified type (if any)</returns>
        protected override IValidator CreateInstance(Type type)
        {
            return _context.Resolve(type) as IValidator;
        }
    }
}
