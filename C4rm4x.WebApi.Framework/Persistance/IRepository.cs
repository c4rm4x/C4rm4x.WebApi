#region Using

using System;
using System.Collections.Generic;
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
        /// Adds a new entity into persistence layer
        /// </summary>
        /// <param name="entityToAdd">Entity to add</param>
        /// <returns>The task</returns>
        Task AddAsync(T entityToAdd);

        /// <summary>
        /// Updates a given entity in persistence layer
        /// </summary>
        /// <param name="entityToUpdate">Entity to update</param>
        /// <returns>The task</returns>
        Task UpdateAsync(T entityToUpdate);

        /// <summary>
        /// Deletes an entity by id
        /// </summary>
        /// <param name="id">Entity's id to be removed</param>
        /// <returns>The task</returns>
        Task DeleteAsync(K id);

        /// <summary>
        /// Deletes a given entity
        /// </summary>
        /// <param name="entityToDelete">Entity to delete</param>
        /// <returns>The task</returns>
        Task DeleteAsync(T entityToDelete);

        /// <summary>
        /// Retrieves the entity of type T with given Id
        /// </summary>
        /// <param name="id">Entity's id to be retrieved</param>
        /// <returns>Entity with given Id</returns>
        /// <returns>The task with the entity</returns>
        Task<T> GetAsync(K id);

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
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Retrieves all the entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The task with the list of all entities that fulfill a given predicate. Empty list if none of them does</returns>
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

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
