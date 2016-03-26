#region Using

using C4rm4x.Tools.Security.Jwt;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Controllers
{
    /// <summary>
    /// Interface responsible for creating instances of JwtGenerationOptions
    /// </summary>
    public interface IJwtGenerationOptionsFactory
    {
        /// <summary>
        /// Gets an instance of JwtGenerationOptions
        /// </summary>
        JwtGenerationOptions GetOptions();
    }
}
