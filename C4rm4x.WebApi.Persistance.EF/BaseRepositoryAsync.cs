#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Persistance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Persistance.EF
{
    /// <summary>
    /// Base implementation of IRepository using EntityFramwork with asycn features
    /// </summary>
    /// <typeparam name="T">Type of the entity</typeparam>
    /// <typeparam name="K">Type of the id</typeparam>
    /// <typeparam name="C">Type of db context</typeparam>
    public class BaseRepositoryAsync<T, K, C> : BaseRepository<T, K, C>
        where T : class
        where C : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entities">The dbContext</param>
        public BaseRepositoryAsync(C entities)
            : base(entities)
        { }

        /// <summary>
        /// Retrieves the entity of type T with given Id
        /// </summary>
        /// <param name="id">Entity's id to be retrieved</param>
        /// <returns>Task with the entity with given Id</returns>
        /// <exception cref="PersistenceException">If there is no entity with given Id</exception>
        public async Task<T> GetAsync(K id)
        {
            var entity = await _set.FindAsync(id);

            if (entity.IsNull())
                throw new PersistenceException(
                    "Entity with id {0} does not exist".AsFormat(id));

            return entity;
        }

        /// <summary>
        /// Retrieves the first ocurrence of an entity of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The taks with the first ocurrence if at least one entity fulfills a given predicate. Null otherwise</returns>
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _set.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Retrieves all the entities of type T
        /// </summary>
        /// <returns>A task with all the entities of type T</returns>
        public async Task<List<T>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        /// <summary>
        /// Retrieves all the entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>A task with the list of all entities that fulfill a given predicate. Empty list if none of them does</returns>
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _set.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Executes an SP that returns a collection of T
        /// </summary>
        /// <param name="queryName">SQL command or stored procedure</param>
        /// <param name="parameters">Stored procedure parameters</param>
        /// <returns>A task with the collection of T returned by SQL statement</returns>
        /// <exception cref="PersistenceException">If any error occurs</exception>           
        public async Task<List<T>> ExecuteQueryAsync(
            string queryName,
            params SqlParameter[] parameters)
        {
            return await Task.FromResult(ExecuteQuery(queryName, parameters));
        }

        /// <summary>
        /// Executes an SP that does not return any result 
        /// </summary>
        /// <param name="queryName">SQL command or stored procedure</param>
        /// <param name="parameters">Stored procedure paramenters</param>
        /// <returns>A task with the number of records affected by the statement</returns>        
        /// <exception cref="PersistenceException">If any error occurs</exception>     
        public async Task<int> ExecuteNonQueryAsync(
            string queryName,
            params SqlParameter[] parameters)
        {
            return await Task.FromResult(ExecuteNonQuery(queryName, parameters));
        }
    }

    /// <summary>
    /// Base implementation of IRepository using EntityFramework where the Id is an int with async features
    /// </summary>
    /// <typeparam name="T">Type of the entity</typeparam>
    /// <typeparam name="C">Type of db context</typeparam>
    public class BaseRepositoryAsync<T, C> : BaseRepositoryAsync<T, int, C>
        where T : class
        where C : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entities">The dbContext</param>
        public BaseRepositoryAsync(C entities)
            : base(entities)
        { }
    }
}
