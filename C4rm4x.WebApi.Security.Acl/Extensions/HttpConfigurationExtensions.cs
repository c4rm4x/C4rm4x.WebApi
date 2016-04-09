#region Using

using C4rm4x.Tools.Utilities;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Security.Acl
{
    /// <summary>
    /// Set of extensions for HttpConfiguration
    /// </summary>
    public static class HttpConfigurationExtensions
    {
        /// <summary>
        /// Returns an instance of WhiteListConfiguration for the given Http config
        /// </summary>
        /// <param name="config">The http configu</param>
        /// <returns>An instance of WhiteListConfiguration</returns>
        public static AclConfiguration GetAclConfiguration(
            this HttpConfiguration config)
        {
            config.NotNull(nameof(config));

            return new AclConfiguration(config);
        }
    }
}
