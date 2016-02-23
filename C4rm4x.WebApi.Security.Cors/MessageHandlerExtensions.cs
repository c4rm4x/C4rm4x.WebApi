#region Using

using System.Web.Cors;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Security.Cors
{
    /// <summary>
    /// Message handler extensions
    /// </summary>
    public static class MessageHandlerExtensions
    {
        /// <summary>
        /// Configure message handlers to enable CORS based SecurityMessageHandler for all the requests
        /// </summary>
        /// <param name="config">The config</param>
        /// <param name="policy">The CORS policy to be applied</param>
        public static void EnableCors(
            this HttpConfiguration config,
            CorsPolicy policy)
        {
            config.MessageHandlers.Add(
                new CorsBasedSecurityMessageHandler(policy));
        }
    }
}
