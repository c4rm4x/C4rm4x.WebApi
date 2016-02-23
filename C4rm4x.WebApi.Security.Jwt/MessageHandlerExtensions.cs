#region Using

using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Security.Jwt
{
    /// <summary>
    /// Message handler extensions
    /// </summary>
    public static class MessageHandlerExtensions
    {
        /// <summary>
        /// Configure message handlers to use JwtBasedSecurityMessageHandler as 
        /// SecurityMessageHandler for all the requests
        /// </summary>
        /// <param name="config">The config</param>
        /// <param name="options">The jwt validation options</param>
        /// <param name="forceAuthentication">Indicates whether or not authentication must be enforced</param>
        public static void UseJwtAuthentication(
            this HttpConfiguration config,
            JwtValidationOptions options,
            bool forceAuthentication = false)
        {
            config.MessageHandlers.Add(
                new JwtBasedSecurityMessageHandler(options, forceAuthentication));
        }
    }
}
