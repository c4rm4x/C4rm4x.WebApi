#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Persistance;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

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
        public async Task<int> CommitAsync()
        {
            return await _entities.SaveChangesAsync();
        }

        /// <summary>
        /// Undoes all changes pending to avoid to persist them 
        /// </summary>
        public async Task RollbackAsync()
        {
            await Task.Run(() =>
            {
                throw new NotImplementedException();
            });
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
