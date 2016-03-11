#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Persistance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

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
        protected readonly DbSet<T> _set;

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
        public void Add(T entityToAdd)
        {
            _set.Add(entityToAdd);
        }

        /// <summary>
        /// Updates a given entity in persistence layer
        /// </summary>
        /// <param name="entityToUpdate">Entity to update</param>
        public void Update(T entityToUpdate)
        {
            _entities.Entry(entityToUpdate).State =
                EntityState.Modified;
        }

        /// <summary>
        /// Deletes an entity by id
        /// </summary>
        /// <param name="id">Entity's id to be removed</param>
        /// <exception cref="PersistenceException">If there is no entity with given Id</exception>
        public void Delete(K id)
        {
            var entityToDelete = Get(id);

            Delete(entityToDelete);
        }

        /// <summary>
        /// Deletes a given entity
        /// </summary>
        /// <param name="entityToDelete">Entity to delete</param>
        public void Delete(T entityToDelete)
        {
            _set.Remove(entityToDelete);
        }

        /// <summary>
        /// Retrieves the entity of type T with given Id
        /// </summary>
        /// <param name="id">Entity's id to be retrieved</param>
        /// <returns>Entity with given Id</returns>
        /// <exception cref="PersistenceException">If there is no entity with given Id</exception>
        public T Get(K id)
        {
            var entity = _set.Find(id);

            if (entity.IsNull())
                throw new PersistenceException(
                    "Entity with id {0} does not exist".AsFormat(id));

            return entity;
        }

        /// <summary>
        /// Retrieves the first ocurrence of an entity of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The first ocurrence if at least one entity fulfills a given predicate. Null otherwise</returns>
        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _set.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Retrieves all the entities of type T
        /// </summary>
        /// <returns>All the entities of type T</returns>
        public List<T> GetAll()
        {
            return _set.ToList();
        }

        /// <summary>
        /// Retrieves all the entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The list of all entities that fulfill a given predicate. Empty list if none of them does</returns>
        public List<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate).ToList();
        }

        /// <summary>
        /// Executes an SP that returns a collection of T
        /// </summary>
        /// <param name="queryName">SQL command or stored procedure</param>
        /// <param name="parameters">Stored procedure parameters</param>
        /// <returns>A collection of T returned by SQL statement</returns>
        /// <exception cref="PersistenceException">If any error occurs</exception>           
        public List<T> ExecuteQuery(
            string queryName,
            params SqlParameter[] parameters)
        {
            try
            {
                return _set.SqlQuery(
                    BuildQuery(queryName, parameters), parameters)
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception e)
            {
                throw new PersistenceException(
                    "Error executing query", e);
            }
        }

        /// <summary>
        /// Executes an SP that does not return any result 
        /// </summary>
        /// <param name="queryName">SQL command or stored procedure</param>
        /// <param name="parameters">Stored procedure paramenters</param>
        /// <returns>The number of records affected by the statement</returns>        
        /// <exception cref="PersistenceException">If any error occurs</exception>     
        public int ExecuteNonQuery(
            string queryName,
            params SqlParameter[] parameters)
        {
            try
            {
                return _entities.Database.ExecuteSqlCommand(
                    BuildQuery(queryName, parameters), parameters);
            }
            catch (Exception e)
            {
                throw new PersistenceException(
                    "Error executing non query", e);
            }
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
