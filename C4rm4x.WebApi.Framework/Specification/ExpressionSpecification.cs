#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Specification
{
    /// <summary>
    /// Specification defined as an expression that must be return true in order to 
    /// satisfy this specification
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity to validate</typeparam>
    public abstract class ExpressionSpecification<TEntity> :
        ISpecification<TEntity>
    {
        private readonly Func<TEntity, bool> _expression;

        /// <summary>
        /// The rule descriptor
        /// </summary>
        public IRule Rule { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rule">The rule descriptor</param>
        /// <param name="expression">Expression that must return true in order to fulfill this specification</param>
        public ExpressionSpecification(
            IRule rule,
            Func<TEntity, bool> expression)
        {
            rule.NotNull(nameof(rule));
            expression.NotNull(nameof(expression));

            Rule = rule;
            _expression = expression;
        }

        /// <summary>
        /// Checks whether the specified entity satisfies the expression
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>True when entity satisfies expression; false otherwise</returns>
        public async Task<bool> IsSatisfiedByAsync(TEntity entity)
        {
            return await Task.FromResult(_expression(entity));
        }
    }
}
