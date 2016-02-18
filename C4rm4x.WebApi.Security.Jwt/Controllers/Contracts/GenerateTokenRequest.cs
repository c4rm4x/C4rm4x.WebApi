#region Using

using C4rm4x.WebApi.Framework;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Controllers
{
    /// <summary>
    /// Generate token request
    /// </summary>
    /// <remarks>Only user identifier is required</remarks>
    [DataContract]
    public class GenerateTokenRequest : ApiRequest
    {
        /// <summary>
        /// Parameterless constructor for serialization/deserialization
        /// </summary>
        public GenerateTokenRequest() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userIdentifier">User identifier</param>
        /// <param name="secret">User's secret</param>
        public GenerateTokenRequest(
            string userIdentifier,
            string secret = null)
        {
            UserIdentifier = userIdentifier;
            Secret = secret;
        }

        /// <summary>
        /// User identifier
        /// </summary>
        /// <remarks>It is required</remarks>
        [DataMember(IsRequired = true)]
        public string UserIdentifier { get; set; }

        /// <summary>
        /// User's secret
        /// </summary>
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string Secret { get; set; }
    }
}
