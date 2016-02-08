#region Using

using C4rm4x.Tools.Utilities;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Specification
{
    /// <summary>
    /// Abstraction of Specification design pattern
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity to validate</typeparam>
    public abstract class AbstractSpecification<TEntity>
        : ISpecification<TEntity>
    {
        private readonly Func<TEntity, Exception> _exceptionToThrow =
            (e) => new Exception();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exceptioToThrow">Exception to be thrown when entity does not fulfill the business rule</param>
        public AbstractSpecification(Func<TEntity, Exception> exceptioToThrow)
        {
            exceptioToThrow.NotNull(nameof(exceptioToThrow));

            _exceptionToThrow = exceptioToThrow;
        }

        /// <summary>
        /// Checks whether or not the specified entity satisfies the business rule. 
        /// When the underlying business rule is not fulfilled, throws an exception
        /// </summary>
        /// <exception cref="Exception">An exception must be thrown when entity does not satisfy the business rule</exception>
        /// <param name="entity">Entity to validate</param>
        public void EnforceRule(TEntity entity)
        {
            entity.NotNull(nameof(entity));

            if (!IsSatisfiedBy(entity))
                throw _exceptionToThrow(entity);
        }

        /// <summary>
        /// Checks whether the specified entity satisfies the business rule
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>True when entity satisfies the business rule; false otherwise</returns>
        public abstract bool IsSatisfiedBy(TEntity entity);
    }
}
