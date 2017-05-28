#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Persistance;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Persistance.Document
{
    /// <summary>
    /// Base implementation of IRepository using DocumentClient
    /// </summary>
    /// <typeparam name="T">Type of the entity</typeparam>
    public class BaseRepository<T> : IRepository<T, string>
        where T : BaseEntity
    {
        private readonly Database _database;
        private readonly IDocumentClient _client;

        private Lazy<DocumentCollection> Collection { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">The instance that implements IDocumentClient</param>
        /// <param name="database">The database (must be created before this)</param>
        public BaseRepository(
            IDocumentClient client,
            Database database)
        {
            client.NotNull(nameof(client));
            database.NotNull(nameof(database));

            _client = client;
            _database = database;

            Collection = new Lazy<DocumentCollection>(() =>
            {
                return GetOrCreateCollection().Result;
            });
        }
        
        private async Task<DocumentCollection> GetOrCreateCollection()
        {
            var collectionName = typeof(T).Name;

            var collection = _client
                .CreateDocumentCollectionQuery(_database.SelfLink)
                .AsEnumerable()
                .FirstOrDefault(c => c.Id == collectionName);

            return collection ??
                await _client.CreateDocumentCollectionAsync(
                    _database.SelfLink, 
                    new DocumentCollection { Id = collectionName });
        }

        private string CollectionDocumentsLink => Collection.Value.DocumentsLink;

        /// <summary>
        /// Adds a new entity into persistance layer
        /// </summary>
        /// <param name="entityToAdd">Entity to add</param>
        public async Task AddAsync(T entityToAdd)
        {
            var result = await _client.CreateDocumentAsync(CollectionDocumentsLink, entityToAdd);

            if (entityToAdd.Id.IsNullOrEmpty())
                entityToAdd.Id = result.Resource.Id;
        }

        /// <summary>
        /// Returns the total number of entities of type T
        /// </summary>
        /// <returns>The number of entities</returns>
        public async Task<long> CountAsync()
        {
            var response = await CreateDocumentQuery()
                .AsDocumentQuery()
                .ExecuteNextAsync<T>();

            return response.LongCount();
        }

        /// <summary>
        /// Returns the number of all entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The number of all entities that fulfill a given predicate</returns>
        public async Task<long> CountAsync(Expression<Func<T, bool>> predicate)
        {
            var response = await CreateDocumentQuery()
                .Where(predicate)
                .AsDocumentQuery()
                .ExecuteNextAsync<T>();

            return response.LongCount();
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
            var entity = await GetAsync(id);

            await _client.DeleteDocumentAsync(entity?.SelfLink);
        }

        /// <summary>
        /// Retrieves the first ocurrence of an entity of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The first ocurrence if at least one entity fulfills a given predicate. Null otherwise</returns>
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var response = await CreateDocumentQuery()
                .Where(predicate)
                .AsDocumentQuery()
                .ExecuteNextAsync<T>();

            return response.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the entity of type T with given Id
        /// </summary>
        /// <param name="id">Entity's id to be retrieved</param>
        /// <returns>Entity with given Id</returns>
        public async Task<T> GetAsync(string id)
        {
            return await GetAsync(e => e.Id == id);
        }

        /// <summary>
        /// Retrieves all the entities of type T
        /// </summary>
        /// <returns>All the entities of type T</returns>
        public async Task<List<T>> GetAllAsync()
        {
            var response = await CreateDocumentQuery()
                .AsDocumentQuery()
                .ExecuteNextAsync<T>();

            return response.ToList();
        }

        /// <summary>
        /// Retrieves all the entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The list of all entities that fulfill a given predicate. Empty list if none of them does</returns>
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            var response = await CreateDocumentQuery()
                .Where(predicate)
                .AsDocumentQuery()
                .ExecuteNextAsync<T>();

            return response.ToList();
        }

        private IOrderedQueryable<T> CreateDocumentQuery()
        {
            return _client.CreateDocumentQuery<T>(CollectionDocumentsLink);
        }

        /// <summary>
        /// Updates a given entity in persistence layer
        /// </summary>
        /// <param name="entityToUpdate">Entity to update</param>
        public async Task UpdateAsync(T entityToUpdate)
        {
            var selfLink = entityToUpdate.SelfLink;

            await _client.ReplaceDocumentAsync(selfLink, entityToUpdate);
        }
    }
}
