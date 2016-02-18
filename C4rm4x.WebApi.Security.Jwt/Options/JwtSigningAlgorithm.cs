namespace C4rm4x.WebApi.Security.Jwt
{
    /// <summary>
    /// Enum with all the possible algorithms to used when signing token is required
    /// </summary>
    public enum JwtSigningAlgorithm
    {
        /// <summary>
        /// No algorithm will be used (plain text)
        /// </summary>
        /// <remarks>DO NOT USE THIS in production environments</remarks>
        NONE,

        /// <summary>
        /// Hash-based message authentication code
        /// </summary>
        HMAC_SHA256
    }
}
