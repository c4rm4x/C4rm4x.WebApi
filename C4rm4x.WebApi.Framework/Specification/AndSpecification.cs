#region Using

using C4rm4x.Tools.Utilities;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Specification
{
    /// <summary>
    /// Specification defined as the combination of 2 others specifications when
    /// both must satisfy each business rule in order to satisfy this
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity to validate</typeparam>
    public abstract class AndSpecification<TEntity> : 
        AbstractSpecification<TEntity>,
        ISpecification<TEntity>
    {
        private readonly ISpecification<TEntity> _left;
        private readonly ISpecification<TEntity> _right;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="left">First specification</param>
        /// <param name="right">Second sepcification</param>
        /// <param name="exceptioToThrow">Exception to be thrown when entity does not fulfill this specification</param>
        public AndSpecification(
            ISpecification<TEntity> left,
            ISpecification<TEntity> right,
            Func<TEntity, Exception> exceptioToThrow):
            base(exceptioToThrow)
        {
            left.NotNull(nameof(left));
            right.NotNull(nameof(right));

            _left = left;
            _right = right;
        }

        /// <summary>
        /// Checks whether the specified entity satisfies both specifications
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>True when entity satisfies boths specifications; false otherwise</returns>
        public override bool IsSatisfiedBy(TEntity entity)
        {
            return _left.IsSatisfiedBy(entity) &&
                _right.IsSatisfiedBy(entity);
        }
    }
}
