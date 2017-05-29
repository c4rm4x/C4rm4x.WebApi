#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Persistance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Persistance.EF
{
    /// <summary>
    /// Base implementation of IRepository using EntityFramwork
    /// </summary>
    /// <typeparam name="T">Type of the entity</typeparam>
    /// <typeparam name="K">Type of the id</typeparam>
    /// <typeparam name="C">Type of db context</typeparam>
    public class BaseRepository<T, K, C> : IRepository<T, K>
        where T : class
        where C : DbContext
    {
        private readonly DbContext _entities;
        private readonly DbSet<T> _set;

        /// <summary>
        /// Gets the dbQuery object linked to this repository
        /// </summary>
        protected DbQuery<T> Query => _set;

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="entities">The dbContext</param>
        public BaseRepository(C entities)
        {
            entities.NotNull(nameof(entities));

            _entities = entities;
            _set = entities.Set<T>();
        }

        /// <summary>
        /// Adds a new entity into persistence layer
        /// </summary>
        /// <param name="entityToAdd">Entity to add</param>
        public Task AddAsync(T entityToAdd)
        {
            return Task.FromResult(_set.Add(entityToAdd));
        }

        /// <summary>
        /// Updates a given entity in persistence layer
        /// </summary>
        /// <param name="entityToUpdate">Entity to update</param>
        public Task UpdateAsync(T entityToUpdate)
        {
            return Task.FromResult(_entities.Entry(entityToUpdate).State = EntityState.Modified);
        }

        /// <summary>
        /// Deletes an entity by id
        /// </summary>
        /// <param name="id">Entity's id to be removed</param>
        public async Task DeleteAsync(K id)
        {
            var entityToDelete = await GetAsync(id);

            await DeleteAsync(entityToDelete);
        }

        /// <summary>
        /// Deletes a given entity
        /// </summary>
        /// <param name="entityToDelete">Entity to delete</param>
        public Task DeleteAsync(T entityToDelete)
        {
            return Task.FromResult(_set.Remove(entityToDelete));
        }

        /// <summary>
        /// Retrieves the entity of type T with given Id
        /// </summary>
        /// <param name="id">Entity's id to be retrieved</param>
        /// <returns>Entity with given Id</returns>
        public async Task<T> GetAsync(K id)
        {
            return await _set.FindAsync(id);
        }

        /// <summary>
        /// Retrieves the first ocurrence of an entity of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The first ocurrence if at least one entity fulfills a given predicate. Null otherwise</returns>
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {            
            return await _set.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Retrieves all the entities of type T
        /// </summary>
        /// <returns>All the entities of type T</returns>
        public async Task<List<T>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        /// <summary>
        /// Retrieves all the entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The list of all entities that fulfill a given predicate. Empty list if none of them does</returns>
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _set.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Returns the number of all entities of type T
        /// </summary>
        /// <returns>The number of entities</returns>
        public async Task<long> CountAsync()
        {
            return await _set.LongCountAsync();
        }

        /// <summary>
        /// Returns the number of all entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">The predicate</param>
        /// <returns>The number of all entities that fulfill a given predicate</returns>
        public async Task<long> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _set.LongCountAsync(predicate);
        }

        /// <summary>
        /// Executes an SP that returns a collection of T
        /// </summary>
        /// <param name="queryName">SQL command or stored procedure</param>
        /// <param name="parameters">Stored procedure parameters</param>
        /// <returns>A collection of T returned by SQL statement</returns>
        public async Task<List<T>> ExecuteQueryAsync(
            string queryName,
            params SqlParameter[] parameters)
        {
            return await _set
                .SqlQuery(BuildQuery(queryName, parameters), parameters)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Executes an SP that does not return any result 
        /// </summary>
        /// <param name="queryName">SQL command or stored procedure</param>
        /// <param name="parameters">Stored procedure paramenters</param>
        /// <returns>The number of records affected by the statement</returns>        
        public async Task<int> ExecuteNonQueryAsync(
            string queryName,
            params SqlParameter[] parameters)
        {
            return await _entities
                .Database
                .ExecuteSqlCommandAsync(BuildQuery(queryName, parameters), parameters);
        }

        private static string BuildQuery(
            string queryName,
            SqlParameter[] parameters)
        {
            return string.Format("exec {0} {1}", queryName,
                string.Join(",", parameters.Select(p =>
                    string.Format("@{0} {1}", p.ParameterName, GetDirection(p))))).Trim();
        }

        private static string GetDirection(SqlParameter parameter)
        {
            return parameter.Direction == ParameterDirection.Output
                ? "out"
                : string.Empty;
        }
    }    

    /// <summary>
    /// Base implementation of IRepository using EntityFramework where the Id is an int
    /// </summary>
    /// <typeparam name="T">Type of the entity</typeparam>
    /// <typeparam name="C">Type of db context</typeparam>
    public class BaseRepository<T, C> : BaseRepository<T, int, C>
        where T : class
        where C : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entities">The dbContext</param>
        public BaseRepository(C entities)
            : base(entities)
        { }
    }    
}
