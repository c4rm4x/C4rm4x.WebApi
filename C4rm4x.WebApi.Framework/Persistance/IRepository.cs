#region Using

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
        void Add(T entityToAdd);

        /// <summary>
        /// Updates a given entity in persistence layer
        /// </summary>
        /// <param name="entityToUpdate">Entity to update</param>
        void Update(T entityToUpdate);

        /// <summary>
        /// Deletes an entity by id
        /// </summary>
        /// <param name="id">Entity's id to be removed</param>
        /// <exception cref="PersistenceException">If there is no entity with given Id</exception>
        void Delete(K id);

        /// <summary>
        /// Deletes a given entity
        /// </summary>
        /// <param name="entityToDelete">Entity to delete</param>
        void Delete(T entityToDelete);

        /// <summary>
        /// Retrieves the entity of type T with given Id
        /// </summary>
        /// <param name="id">Entity's id to be retrieved</param>
        /// <returns>Entity with given Id</returns>
        /// <exception cref="PersistenceException">If there is no entity with given Id</exception>
        T Get(K id);

        /// <summary>
        /// Retrieves the first ocurrence of an entity of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The first ocurrence if at least one entity fulfills a given predicate. Null otherwise</returns>
        T Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Retrieves all the entities of type T
        /// </summary>
        /// <returns></returns>
        List<T> GetAll();

        /// <summary>
        /// Retrieves all the entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The list of all entities that fulfill a given predicate. Empty list if none of them does</returns>
        List<T> GetAll(Expression<Func<T, bool>> predicate);
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
