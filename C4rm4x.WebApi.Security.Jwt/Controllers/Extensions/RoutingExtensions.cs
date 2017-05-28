#region Using

using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Controllers
{
    /// <summary>
    /// Routing extensions
    /// </summary>
    public static class RoutingExtensions
    {
        /// <summary>
        /// Register token controller within your application's HttpConfiguration
        /// </summary>
        /// <param name="config">The config</param>
        /// <param name="routeTemplate">Route template</param>
        public static void RegisterTokenController(
            this HttpConfiguration config,
            string routeTemplate = "api/token")
        {
            config
                .Routes
                .MapHttpRoute("Token", routeTemplate,
                new
                {
                    controller = "Token",
                    action = "GenerateToken"
                });
        }
    }
}
