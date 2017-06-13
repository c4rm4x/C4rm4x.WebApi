#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework;
using C4rm4x.WebApi.Framework.Events;
using C4rm4x.WebApi.Framework.Persistance;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Persistance.EF
{
    /// <summary>
    /// Implementation of ISession using EF DbContext
    /// </summary>
    public class Session : ISession, IDisposable
    {
        private readonly DbContext _entities;

        private readonly IEventStore _store;

        private readonly IEventPublisher _publisher;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entities">The db context</param>
        /// <param name="store">The event store</param>
        /// <param name="publisher">The event publisher</param>
        public Session(
            DbContext entities,
            IEventStore store,
            IEventPublisher publisher)
        {
            entities.NotNull(nameof(entities));
            store.NotNull(nameof(store));
            publisher.NotNull(nameof(publisher));

            _entities = entities;
            _store = store;
            _publisher = publisher;
        }

        /// <summary>
        /// Gets whether or not the unit of work has already been disposed
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Adds the entity into the session
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        /// <param name="entity">The entity to be added</param>
        public void Add<TEntity>(TEntity entity) 
            where TEntity : AggregateRoot
        {
            _entities.Set<TEntity>().Add(entity);
        }

        /// <summary>
        /// Retrieves the entity based on the given id
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        /// <param name="id">The id</param>
        /// <returns>The entity for the given id</returns>
        public Task<TEntity> GetAsync<TEntity>(int id) 
            where TEntity : AggregateRoot
        {
            return GetAsync<TEntity, int>(id);
        }

        /// <summary>
        /// Retrieves the entity based on the given id
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        /// <typeparam name="K">Type of the primary key</typeparam>
        /// <param name="id">The id</param>
        /// <returns>The entity for the given id</returns>
        public Task<TEntity> GetAsync<TEntity, K>(K id) 
            where TEntity : AggregateRoot
        {
            return _entities.Set<TEntity>().FindAsync(id);
        }

        /// <summary>
        /// Saves all pending changes into persistence layer
        /// </summary>
        /// <returns>The task with the number of changes persisted</returns>
        public async Task<int> SaveAllAsync()
        {
            using (var transaction = _entities.Database.BeginTransaction())
            {
                foreach (var entry in _entities.ChangeTracker.Entries<AggregateRoot>())
                {
                    var events = entry.Entity.FlushEvents();

                    await _store.SaveAllAsync(events);

                    await Task.WhenAll(events.Select(@event => _publisher.PublishAsync(@event)));
                }

                var result = await _entities.SaveChangesAsync();

                transaction.Commit();

                return result;
            }
        }

        /// <summary>
        /// Disposes the db context
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;

            _entities.Dispose();

            GC.SuppressFinalize(this);

            IsDisposed = true;
        }
    }
}
