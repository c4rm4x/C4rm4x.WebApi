#region Using

using C4rm4x.Tools.Utilities;
using Microsoft.Azure.Documents;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Persistance.Document
{
    #region Interface

    /// <summary>
    /// Service responsible to provide a DocumentDb database (or create it when required)
    /// </summary>
    public interface IDatabaseInitialiser
    {
        /// <summary>
        /// Get or create (when required) database for the given identifier
        /// </summary>
        /// <param name="id">The database identifier</param>
        /// <returns>The instance of DocumentDB database</returns>
        Database GetOrCreateDatabase(string id);
    }

    #endregion

    /// <summary>
    /// Implementation of IDatabaseInitialiser
    /// </summary>
    public class DatabaseInitialiser : IDatabaseInitialiser
    {
        private readonly IDocumentClient _client;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">The instance that implements IDocumentClient</param>
        public DatabaseInitialiser(
            IDocumentClient client)
        {
            client.NotNull(nameof(client));

            _client = client;
        }

        /// <summary>
        /// Get or create (when required) database for the given identifier
        /// </summary>
        /// <param name="id">The database identifier</param>
        /// <returns>The instance of DocumentDB database</returns>
        public Database GetOrCreateDatabase(string id)
        {
            var db = _client.CreateDatabaseQuery().AsEnumerable().FirstOrDefault(d => d.Id == id);

            return db ?? _client.CreateDatabaseAsync(new Database { Id = id }).Result;
        }
    }
}
