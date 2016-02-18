namespace C4rm4x.WebApi.Security.Jwt.Controller
{
    /// <summary>
    /// Interface responsible for creating instances of JwtGenerationOptions
    /// </summary>
    public interface IJwtGenerationOptionsFactory
    {
        /// <summary>
        /// Gets an instance of JwtGenerationOptions
        /// </summary>
        /// <returns></returns>
        JwtGenerationOptions GetOptions();
    }
}
