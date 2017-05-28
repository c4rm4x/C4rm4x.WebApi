#region Using

using C4rm4x.Tools.Utilities;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Controllers
{
    /// <summary>
    /// Generate token response
    /// </summary>
    [DataContract]
    public class GenerateTokenResponse
    {
        /// <summary>
        /// Parameterless constructor for serialization/deserialization
        /// </summary>
        public GenerateTokenResponse() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">The token</param>
        public GenerateTokenResponse(string token)
        {
            token.NotNullOrEmpty(nameof(token));

            Token = token;
        }

        /// <summary>
        /// The JWT generated
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Token { get; set; }
    }
}
