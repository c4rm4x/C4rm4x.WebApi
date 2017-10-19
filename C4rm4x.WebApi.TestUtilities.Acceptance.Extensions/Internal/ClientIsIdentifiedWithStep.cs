#region Using

using C4rm4x.Tools.Security.Acl;
using C4rm4x.Tools.Utilities;

#endregion

namespace C4rm4x.WebApi.TestUtilities.Acceptance.Internal
{
    /// <summary>
    /// Given-when-then step to generate an base-64 Basic authorization token with the given identifier/secret pair
    /// </summary>
    internal class ClientIsIdentifiedWithStep
    {
        /// <summary>
        /// Gets the Http server responsible to handle requests
        /// </summary>
        public InMemoryHttpServer HttpServer { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpServer">The Http server</param>
        public ClientIsIdentifiedWithStep(InMemoryHttpServer httpServer)
        {
            httpServer.NotNull(nameof(httpServer));

            HttpServer = httpServer;
        }

        /// <summary>
        /// Generate a basic token with the given identifier/secret pair and sets this
        /// at Http server to include it as a Authorization header for each request
        /// </summary>
        /// <param name="identifier">The identifier</param>
        /// <param name="secret">The shared secret (as base-64)</param>
        internal void AuthorizationFor(
            string identifier,
            string secret)
        {
            var token = new AclClientCredentialsGenerator()
                .Generate(identifier, secret);

            HttpServer.SetSecurityToken(token);
        }
    }
}
