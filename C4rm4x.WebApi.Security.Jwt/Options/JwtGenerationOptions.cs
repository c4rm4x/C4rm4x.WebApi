namespace C4rm4x.WebApi.Security.Jwt
{
    /// <summary>
    /// Generation JWT options
    /// </summary>
    public class JwtGenerationOptions
    {
        private const double DefaultTokenLifetimeInMinutes = 60;

        /// <summary>
        /// Gets the lifetime of token IN minutes
        /// </summary>
        public double TokenLifetimeInMinutes { get; private set; }

        /// <summary>
        /// Gets the JWT signing options used to sign the token (when required)
        /// </summary>
        public JwtSigningCredentials SigningCredentials { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tokenLifetimeInMinutes">Token lifetime IN minutes</param>
        /// <param name="signingCredentials">Signing credentials (when required)</param>
        public JwtGenerationOptions(
            double tokenLifetimeInMinutes = DefaultTokenLifetimeInMinutes,
            JwtSigningCredentials signingCredentials = null)
        {
            TokenLifetimeInMinutes = tokenLifetimeInMinutes;
            SigningCredentials = signingCredentials;
        }
    }
}
