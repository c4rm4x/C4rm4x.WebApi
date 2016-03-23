#region Using

using C4rm4x.Tools.Utilities;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Specification
{
    /// <summary>
    /// Specification that verifies the entity is not null
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity to validate</typeparam>
    public abstract class NotNullSpecification<TEntity> : 
        AbstractSpecification<TEntity>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exceptionToThrow">Exception to be thrown when entity does not fulfill the business rule</param>
        public NotNullSpecification(Func<TEntity, Exception> exceptionToThrow)
            : base(exceptionToThrow)
        {
        }

        /// <summary>
        /// Checks whether or not the entity is not null
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>True when entity is not null; false otherwise</returns>
        public override bool IsSatisfiedBy(TEntity entity)
        {
            return entity.IsNotNull();
        }
    }
}
