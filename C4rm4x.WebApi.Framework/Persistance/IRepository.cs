#region Using

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Persistance
{
    /// <summary>
    /// Service responsible to liase with data access for specified type of entity
    /// </summary>
    /// <typeparam name="T">Type of the entity</typeparam>
    /// <typeparam name="K">Type of the id</typeparam>
    public interface IRepository<T, K>
            where T : class
    {
        /// <summary>
        /// Retrieves the first ocurrence of an entity of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The first ocurrence if at least one entity fulfills a given predicate. Null otherwise</returns>
        /// <returns>The task with the entity</returns>
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Retrieves all the entities of type T
        /// </summary>
        /// <returns>The task with all the entities</returns>
        Task<IQueryable<T>> GetAllAsync();

        /// <summary>
        /// Retrieves all the entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The task with the list of all entities that fulfill a given predicate. Empty list if none of them does</returns>
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Returns the number of all entities of type T
        /// </summary>
        /// <returns>The task with the number of entities</returns>
        Task<long> CountAsync();

        /// <summary>
        /// Returns the number of all entities of type T based on predicate 
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The task with the number of all entities that fulfill a given predicate</returns>
        Task<long> CountAsync(Expression<Func<T, bool>> predicate);
    }

    /// <summary>
    /// Service responsible to liase with data access for the specified type of 
    /// entity with integer id
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    public interface IRepository<T> : IRepository<T, int>
        where T : class
    {
    }
}
