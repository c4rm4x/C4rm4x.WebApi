#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Persistance;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Persistance.EF
{
    /// <summary>
    /// Base implementation of IUnitOfWork using EntityFramework
    /// </summary>
    public abstract class UnitOfWork :
        IUnitOfWork, IDisposable
    {
        private readonly DbContext _entities;

        /// <summary>
        /// Gets whether or not the unit of work has already been disposed
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entities">The db context</param>
        public UnitOfWork(DbContext entities)
        {
            entities.NotNull(nameof(entities));

            _entities = entities;
        }

        /// <summary>
        /// Saves all pending changes into persistence layer
        /// </summary>
        /// <returns>Returns the number of changes persisted</returns>
        public int Commit()
        {
            return _entities.SaveChanges();
        }

        /// <summary>
        /// Undoes all changes pending to avoid to persist them 
        /// </summary>
        public void Rollback()
        {
            Rollback(EntityState.Added, EntityState.Detached);
            Rollback(EntityState.Deleted, EntityState.Unchanged);
            Rollback(EntityState.Modified, EntityState.Unchanged,
                e => e.CurrentValues.SetValues(e.OriginalValues));
        }

        private void Rollback(
            EntityState currentState,
            EntityState newState,
            Action<DbEntityEntry> action = null)
        {
            foreach (var entry in _entities.ChangeTracker.Entries()
                .Where(e => e.State == currentState))
            {
                if (action != null) action(entry);

                entry.State = newState;
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
