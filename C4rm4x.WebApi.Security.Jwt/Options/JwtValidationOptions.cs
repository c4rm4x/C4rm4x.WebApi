namespace C4rm4x.WebApi.Security.Jwt
{
    /// <summary>
    /// Validation JWT options
    /// </summary>
    public class JwtValidationOptions
    {
        /// <summary>
        /// Gets whether JWT lifetime must be validated
        /// </summary>
        public bool RequireExpirationTime { get; private set; }

        /// <summary>
        /// Gets the signing key used to validate signature
        /// </summary>
        public string SigningKey { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requireExpirationTime">Indicates whether JWT lifetime must be validated</param>
        /// <param name="signingKey">Signing key used to validate signature</param>
        public JwtValidationOptions(
            bool requireExpirationTime = true,
            string signingKey = null)
        {
            RequireExpirationTime = requireExpirationTime;
            SigningKey = signingKey;
        }
    }
}
