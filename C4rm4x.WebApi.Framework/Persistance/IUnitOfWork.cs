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
        /// <returns>Returns the number of changes persisted</returns>
        int Commit();

        /// <summary>
        /// Undoes all changes pending to avoid to persist them 
        /// </summary>
        void Rollback();
    }
}
