﻿#region Using

using C4rm4x.WebApi.Framework.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// <param name="errors">The errors</param>
        /// <returns>True when entity satisfies the business rule; false otherwise</returns>
        Task<bool> IsSatisfiedByAsync(
            TEntity entity,
            ICollection<ValidationError> errors);
    }
}
