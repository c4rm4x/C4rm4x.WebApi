#region Using

using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Persistance
{
    /// <summary>
    /// A Unit of Work keeps track of everything you do during a business 
    /// transaction that can affect the database
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Saves all pending changes into persistence layer
        /// </summary>
        /// <returns>The task with the number of changes persisted</returns>
        Task<int> CommitAsync();

        /// <summary>
        /// Undoes all changes pending to avoid to persist them 
        /// </summary>
        /// <returns>The task</returns>
        Task RollbackAsync();
    }
}
