#region Using

using C4rm4x.Tools.Utilities;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Specification
{
    /// <summary>
    /// Specification defined as an expression that must be return true in order to 
    /// satisfy this specification
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity to validate</typeparam>
    public abstract class ExpressionSpecification<TEntity> :
        AbstractSpecification<TEntity>,
        ISpecification<TEntity>
    {
        private readonly Func<TEntity, bool> _expression;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="expression">Expression that must return true in order to fulfill this specification</param>
        /// <param name="exceptioToThrow">Exception to be thrown when entity does not fulfill the business rule</param>
        public ExpressionSpecification(
            Func<TEntity, bool> expression,
            Func<TEntity, Exception> exceptioToThrow) : base(exceptioToThrow)
        {
            expression.NotNull(nameof(expression));

            _expression = expression;
        }

        /// <summary>
        /// Checks whether the specified entity satisfies the expression
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>True when entity satisfies expression; false otherwise</returns>
        public override bool IsSatisfiedBy(TEntity entity)
        {
            return _expression(entity);
        }
    }
}
