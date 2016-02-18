namespace C4rm4x.WebApi.Security.Jwt
{
    /// <summary>
    /// Signing JWT options (when signed tokens are required)
    /// </summary>
    public class JwtSigningCredentials
    {
        /// <summary>
        /// Gets the signing algorithm that will be used to sign the final token
        /// </summary>
        public JwtSigningAlgorithm SigningAlgorithm { get; private set; }

        /// <summary>
        /// Gets the signing key that will be used to sign the final token
        /// </summary>
        /// <remarks>This IS a base64 encoded string</remarks>
        public string Key { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="algorithm">Signing algorithm to use when signing token</param>
        /// <param name="key">Key AS BASE64 encoded string used for signing token</param>
        public JwtSigningCredentials(
            JwtSigningAlgorithm algorithm = JwtSigningAlgorithm.NONE,
            string key = null)
        {
            Key = key;
            SigningAlgorithm = algorithm;
        }
    }
}
