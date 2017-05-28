#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Specification
{
    /// <summary>
    /// Specification defined as the combination of 2 others specifications when
    /// at least one of them must satisfy their business rule in order to satisfy this
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity to validate</typeparam>
    public abstract class OrSpecification<TEntity> : 
        ISpecification<TEntity>
    {
        private readonly ISpecification<TEntity> _left;
        private readonly ISpecification<TEntity> _right;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="left">First specification</param>
        /// <param name="right">Second sepcification</param>
        public OrSpecification(
            ISpecification<TEntity> left,
            ISpecification<TEntity> right)
        {
            left.NotNull(nameof(left));
            right.NotNull(nameof(right));

            _left = left;
            _right = right;
        }

        /// <summary>
        /// Checks whether the specified entity satisfies at least one of the specifications
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <param name="errors">The errors</param>
        /// <returns>True when entity satisfies at least one of the speficications; false otherwise</returns>
        public async Task<bool> IsSatisfiedByAsync(
            TEntity entity,
            ICollection<ValidationError> errors)
        {
            return 
                await _left.IsSatisfiedByAsync(entity, errors) ||
                await _right.IsSatisfiedByAsync(entity, errors);
        }
    }
}
