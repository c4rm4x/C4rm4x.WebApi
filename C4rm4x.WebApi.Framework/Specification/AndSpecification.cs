#region Using

using C4rm4x.Tools.Utilities;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Specification
{
    /// <summary>
    /// Specification defined as the combination of 2 others specifications when
    /// both must satisfy each business rule in order to satisfy this
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity to validate</typeparam>
    public abstract class AndSpecification<TEntity> : 
        ISpecification<TEntity>
    {
        private readonly ISpecification<TEntity> _left;
        private readonly ISpecification<TEntity> _right;

        /// <summary>
        /// The rule descriptor
        /// </summary>
        public IRule Rule { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rule">The rule descriptor</param>
        /// <param name="left">First specification</param>
        /// <param name="right">Second sepcification</param>
        public AndSpecification(
            IRule rule,
            ISpecification<TEntity> left,
            ISpecification<TEntity> right)
        {
            rule.NotNull(nameof(rule));
            left.NotNull(nameof(left));
            right.NotNull(nameof(right));

            Rule = rule;
            _left = left;
            _right = right;
        }

        /// <summary>
        /// Checks whether the specified entity satisfies both specifications
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>True when entity satisfies boths specifications; false otherwise</returns>
        public async Task<bool> IsSatisfiedByAsync(TEntity entity)
        {
            return 
                await _left.IsSatisfiedByAsync(entity) &&
                await _right.IsSatisfiedByAsync(entity);            
        }
    }
}
