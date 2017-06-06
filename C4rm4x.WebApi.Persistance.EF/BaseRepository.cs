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
        private readonly DbSet<T> _set;
        private readonly DbContext _entities;

        /// <summary>
        /// Gets the dbQuery object linked to this repository with tracking disabled
        /// </summary>
        protected DbQuery<T> Query => _set.AsNoTracking();

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
        /// Retrieves the first ocurrence of an entity of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The first ocurrence if at least one entity fulfills a given predicate. Null otherwise</returns>
        public Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {            
            return _set.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Retrieves all the entities of type T
        /// </summary>
        /// <returns>All the entities of type T</returns>
        public Task<IQueryable<T>> GetAllAsync()
        {
            return Task.FromResult(_set.AsNoTracking().AsQueryable());
        }

        /// <summary>
        /// Retrieves all the entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The list of all entities that fulfill a given predicate. Empty list if none of them does</returns>
        public Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return Task.FromResult(_set.AsNoTracking().Where(predicate).AsQueryable());
        }

        /// <summary>
        /// Returns the number of all entities of type T
        /// </summary>
        /// <returns>The number of entities</returns>
        public Task<long> CountAsync()
        {
            return _set.LongCountAsync();
        }

        /// <summary>
        /// Returns the number of all entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">The predicate</param>
        /// <returns>The number of all entities that fulfill a given predicate</returns>
        public Task<long> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return _set.LongCountAsync(predicate);
        }

        /// <summary>
        /// Executes an SP that returns a collection of T
        /// </summary>
        /// <param name="queryName">SQL command or stored procedure</param>
        /// <param name="parameters">Stored procedure parameters</param>
        /// <returns>A collection of T returned by SQL statement</returns>
        public Task<List<T>> ExecuteQueryAsync(
            string queryName,
            params SqlParameter[] parameters)
        {
            return _set
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
        public Task<int> ExecuteNonQueryAsync(
            string queryName,
            params SqlParameter[] parameters)
        {
            return _entities
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
