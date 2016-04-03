#region Using

using C4rm4x.Tools.Utilities;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache
{
    /// <summary>
    /// Extensions for instances of the class HttpConfiguration
    /// </summary>
    public static class HttpConfigurationExtensions
    {
        /// <summary>
        /// Returns an instance of OuputCacheConfiguration for the given Http config
        /// </summary>
        /// <param name="config">The http configu</param>
        /// <returns>An instance of OutputCacheConfiguration</returns>
        public static OutputCacheConfiguration GetOutputCacheConfiguration(
            this HttpConfiguration config)
        {
            config.NotNull(nameof(config));

            return new OutputCacheConfiguration(config);
        }
    }
}
