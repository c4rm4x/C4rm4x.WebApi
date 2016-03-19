#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Persistance;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace C4rm4x.WebApi.Persistance.Mongo
{
    /// <summary>
    /// Base implementation of IRepository using MongoCollections
    /// </summary>
    /// <typeparam name="T">Type of the entity</typeparam>
    public class BaseRepository<T> : IRepository<T, string>
        where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="database">The Mongo database</param>
        public BaseRepository(IMongoDatabase database)
        {
            database.NotNull(nameof(database));

            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        /// <summary>
        /// Adds a new entity into persistance layer
        /// </summary>
        /// <param name="entityToAdd">Entity to add</param>
        public void Add(T entityToAdd)
        {
            _collection.InsertOne(entityToAdd);
        }

        /// <summary>
        /// Deletes a given entity
        /// </summary>
        /// <param name="entityToDelete">Entity to delete</param>
        /// <exception cref="PersistenceException">If there is no entity</exception>
        public void Delete(T entityToDelete)
        {
            Delete(entityToDelete.Id);
        }

        /// <summary>
        /// Deletes an entity by id
        /// </summary>
        /// <param name="id">Entity's id to be removed</param>
        /// <exception cref="PersistenceException">If there is no entity with given Id</exception>
        public void Delete(string id)
        {
            Get(id); // Force entity to exists

            _collection.DeleteOne(GetFilter(id));
        }

        /// <summary>
        /// Retrieves the first ocurrence of an entity of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The first ocurrence if at least one entity fulfills a given predicate. Null otherwise</returns>
        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _collection.AsQueryable().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Retrieves the entity of type T with given Id
        /// </summary>
        /// <param name="id">Entity's id to be retrieved</param>
        /// <returns>Entity with given Id</returns>
        /// <exception cref="PersistenceException">If there is no entity with given Id</exception>
        public T Get(string id)
        {
            var entity = Get(GetFilter(id));

            if (entity.IsNull())
                throw new PersistenceException(
                    "Entity with id {0} does not exist".AsFormat(id));

            return entity;
        }

        /// <summary>
        /// Retrieves all the entities of type T
        /// </summary>
        /// <returns>All the entities of type T</returns>
        public List<T> GetAll()
        {
            return _collection.AsQueryable().ToList();
        }

        /// <summary>
        /// Retrieves all the entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The list of all entities that fulfill a given predicate. Empty list if none of them does</returns>
        public List<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _collection.Find(predicate).ToList();
        }

        /// <summary>
        /// Updates a given entity in persistence layer
        /// </summary>
        /// <param name="entityToUpdate">Entity to update</param>
        public void Update(T entityToUpdate)
        {
            _collection.ReplaceOne(
                GetFilter(entityToUpdate.Id), 
                entityToUpdate);
        }

        /// <summary>
        /// Returns the total number of entities of type T
        /// </summary>
        /// <returns>The number of entities</returns>
        public long Count()
        {
            return _collection.AsQueryable().LongCount();
        }

        /// <summary>
        /// Returns the number of all entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The number of all entities that fulfill a given predicate</returns>
        public long Count(Expression<Func<T, bool>> predicate)
        {
            return _collection.Count(predicate);
        }

        private static Expression<Func<T, bool>> GetFilter(string entityId)
        {
            return e => e.Id == entityId;
        }
    }
}
