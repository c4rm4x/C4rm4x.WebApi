#region Using

using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Configuration
{
    /// <summary>
    /// App configuration repository
    /// </summary>
    public interface IAppConfigurationRepository
    {
        /// <summary>
        /// Retrieve the configuraiton for the given app (identifier/version)
        /// </summary>
        /// <param name="appIdentifier">The app identifier</param>
        /// <param name="version">The app version</param>
        /// <returns>The configuration for the given app</returns>
        Task<AppConfiguration> GetConfigurationAsync(string appIdentifier, string version);
    }
}
