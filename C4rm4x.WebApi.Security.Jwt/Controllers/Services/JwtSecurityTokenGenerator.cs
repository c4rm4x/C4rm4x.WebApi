#region Using

using C4rm4x.Tools.Security.Jwt;
using System.Security.Claims;
using JwtGenerator = C4rm4x.Tools.Security.Jwt.JwtSecurityTokenGenerator;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Controllers
{
    #region Interface

    /// <summary>
    /// Interface of the Json web tokenn generator
    /// </summary>
    public interface IJwtSecurityTokenGenerator
    {
        /// <summary>
        /// Generates an instance of JWT as a JSON Compact serialized format string
        /// </summary>
        /// <param name="subject">The source of the claim(s) for this token</param>
        /// <param name="options">Options to generate the JWT</param>
        /// <returns>The JWT as a signed (if required) encoded string.</returns>
        string Generate(
            ClaimsIdentity subject,
            JwtGenerationOptions options);
    }

    #endregion

    /// <summary>
    /// Implementation of the Json web tokens generator
    /// </summary>
    public class JwtSecurityTokenGenerator : IJwtSecurityTokenGenerator
    {
        /// <summary>
        /// Generates an instance of JWT as a JSON Compact serialized format string
        /// </summary>
        /// <param name="subject">The source of the claim(s) for this token</param>
        /// <param name="options">Options to generate the JWT</param>
        /// <returns>The JWT as a signed (if required) encoded string.</returns>
        public string Generate(
            ClaimsIdentity subject, 
            JwtGenerationOptions options)
        {
            return new JwtGenerator().Generate(subject, options);
        }
    }
}
