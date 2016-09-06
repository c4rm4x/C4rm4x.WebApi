#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Persistance;
using Microsoft.Azure.Documents;
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
        }

        private Lazy<DocumentCollection> Collection =>
            new Lazy<DocumentCollection>(() =>
            {
                return GetOrCreateCollection().Result;
            });

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
        public void Add(T entityToAdd)
        {
            var result = _client.CreateDocumentAsync(CollectionDocumentsLink, entityToAdd).Result;

            if (entityToAdd.Id.IsNullOrEmpty())
                entityToAdd.Id = result.Resource.Id;
        }

        /// <summary>
        /// Returns the total number of entities of type T
        /// </summary>
        /// <returns>The number of entities</returns>
        public long Count()
        {
            return CreateDocumentQuery().AsEnumerable().LongCount();
        }

        /// <summary>
        /// Returns the number of all entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The number of all entities that fulfill a given predicate</returns>
        public long Count(Expression<Func<T, bool>> predicate)
        {
            return CreateDocumentQuery().Where(predicate).AsEnumerable().LongCount();
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
            var selfLink = Get(id).SelfLink;

            var result = _client.DeleteDocumentAsync(selfLink).Result;
        }

        /// <summary>
        /// Retrieves the first ocurrence of an entity of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The first ocurrence if at least one entity fulfills a given predicate. Null otherwise</returns>
        public T Get(Expression<Func<T, bool>> predicate)
        {
            return CreateDocumentQuery().Where(predicate).AsEnumerable().FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the entity of type T with given Id
        /// </summary>
        /// <param name="id">Entity's id to be retrieved</param>
        /// <returns>Entity with given Id</returns>
        /// <exception cref="PersistenceException">If there is no entity with given Id</exception>
        public T Get(string id)
        {
            var entity = Get(e => e.Id == id);

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
            return CreateDocumentQuery().ToList();
        }

        /// <summary>
        /// Retrieves all the entities of type T based on predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>The list of all entities that fulfill a given predicate. Empty list if none of them does</returns>
        public List<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return CreateDocumentQuery().Where(predicate).ToList();
        }

        private IOrderedQueryable<T> CreateDocumentQuery()
        {
            return _client.CreateDocumentQuery<T>(CollectionDocumentsLink);
        }

        /// <summary>
        /// Updates a given entity in persistence layer
        /// </summary>
        /// <param name="entityToUpdate">Entity to update</param>
        public void Update(T entityToUpdate)
        {
            var selfLink = entityToUpdate.SelfLink;

            var result = _client.ReplaceDocumentAsync(selfLink, entityToUpdate).Result;
        }
    }
}
