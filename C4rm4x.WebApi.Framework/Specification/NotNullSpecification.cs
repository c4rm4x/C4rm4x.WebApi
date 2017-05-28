#region Using

using C4rm4x.Tools.Utilities;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Specification
{
    /// <summary>
    /// Specification that verifies the entity is not null
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity to validate</typeparam>
    public abstract class NotNullSpecification<TEntity> : 
        ISpecification<TEntity>
    { 
        /// <summary>
        /// Gets the rule descriptor
        /// </summary>
        public IRule Rule { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rule">The rule descriptor</param>
        public NotNullSpecification(IRule rule)
        {
            rule.NotNull(nameof(rule));

            Rule = rule;
        }

        /// <summary>
        /// Checks whether or not the entity is not null
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>True when entity is not null; false otherwise</returns>
        public async Task<bool> IsSatisfiedByAsync(TEntity entity)
        {
            return await Task.FromResult(entity.IsNotNull());
        }
    }
}
