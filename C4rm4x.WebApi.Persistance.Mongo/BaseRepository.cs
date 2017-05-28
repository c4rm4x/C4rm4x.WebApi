#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Persistance;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
        public async Task AddAsync(T entityToAdd)
        {
            await _collection.InsertOneAsync(entityToAdd);
        }

        /// <summary>
        /// Deletes a given entity
        /// </summary>
        /// <param name="entityToDelete">Entity to delete</param>
        public async Task DeleteAsync(T entityToDelete)
        {
            await DeleteAsync(entityToDelete.Id);
        }

        /// <summary>
        /// Deletes an entity by id
        /// </summary>
        /// <param name="id">Entity's id to be removed</param>
        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(GetFilter(id));
        }

        /// <summary>
        /// Retrieves the first ocurrence of an entity of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The first ocurrence if at least one entity fulfills a given predicate. Null otherwise</returns>
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var response = await _collection.FindAsync(predicate);

            return response.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the entity of type T with given Id
        /// </summary>
        /// <param name="id">Entity's id to be retrieved</param>
        /// <returns>Entity with given Id</returns>
        public async Task<T> GetAsync(string id)
        {
            return await GetAsync(GetFilter(id));
        }

        /// <summary>
        /// Retrieves all the entities of type T
        /// </summary>
        /// <returns>All the entities of type T</returns>
        public async Task<List<T>> GetAllAsync()
        {
            return await _collection.AsQueryable().ToListAsync();
        }

        /// <summary>
        /// Retrieves all the entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The list of all entities that fulfill a given predicate. Empty list if none of them does</returns>
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        /// <summary>
        /// Updates a given entity in persistence layer
        /// </summary>
        /// <param name="entityToUpdate">Entity to update</param>
        public async Task UpdateAsync(T entityToUpdate)
        {
            await _collection.ReplaceOneAsync(
                GetFilter(entityToUpdate.Id), 
                entityToUpdate);
        }

        /// <summary>
        /// Returns the total number of entities of type T
        /// </summary>
        /// <returns>The number of entities</returns>
        public async Task<long> CountAsync()
        {
            return await Task.FromResult(_collection.AsQueryable().LongCount());
        }

        /// <summary>
        /// Returns the number of all entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The number of all entities that fulfill a given predicate</returns>
        public async Task<long> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _collection.CountAsync(predicate);
        }

        private static Expression<Func<T, bool>> GetFilter(string entityId)
        {
            return e => e.Id == entityId;
        }
    }
}
