#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Framework.Specification
{
    /// <summary>
    /// Abstraction of Specification design pattern
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity to validate</typeparam>
    public interface ISpecification<TEntity>
    {
        /// <summary>
        /// Checks whether the specified entity satisfies the business rule
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>True when entity satisfies the business rule; false otherwise</returns>
        bool IsSatisfiedBy(TEntity entity);

        /// <summary>
        /// Checks whether or not the specified entity satisfies the business rule. 
        /// When the underlying business rule is not fulfilled, throws an exception
        /// </summary>
        /// <exception cref="Exception">An exception must be thrown when entity does not satisfy the business rule</exception>
        /// <param name="entity">Entity to validate</param>
        void EnforceRule(TEntity entity);
    }
}
