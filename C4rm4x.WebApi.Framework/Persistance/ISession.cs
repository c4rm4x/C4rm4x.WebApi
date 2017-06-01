#region Using

using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Persistance
{
    /// <summary>
    /// A Unit of Work keeps track of everything you do during a business 
    /// transaction that can affect the persistance
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// Saves all pending changes into persistence layer
        /// </summary>
        /// <returns>The task with the number of changes persisted</returns>
        Task<int> SaveAllAsync();

        /// <summary>
        /// Retrieves the entity based on the given id
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        /// <typeparam name="K">Type of the key</typeparam>
        /// <param name="id">The id</param>
        /// <returns>The entity for the given id</returns>
        Task<TEntity> GetAsync<TEntity, K>(K id)
            where TEntity : AggregateRoot;

        /// <summary>
        /// Retrieves the entity based on the given id
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        /// <param name="id">The id</param>
        /// <returns>The entity for the given id</returns>
        Task<TEntity> GetAsync<TEntity>(int id)
            where TEntity : AggregateRoot;

        /// <summary>
        /// Adds the entity into the session
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        /// <param name="entity">The entity to be added</param>
        Task AddAsync<TEntity>(TEntity entity)
            where TEntity : AggregateRoot;

        /// <summary>
        /// Takes the entity out of the session
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity</typeparam>
        /// <param name="entity">The entity</param>
        Task DeleteAsync<TEntity>(TEntity entity)
            where TEntity : AggregateRoot; 
    }   
}
