#region Using

using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Configuration.Controllers
{
    /// <summary>
    /// Routing extensions
    /// </summary>
    public static class RoutingExtensions
    {
        /// <summary>
        /// Register configuration controller within your application's HttpConfiguration
        /// </summary>
        /// <param name="config">The config</param>
        /// <param name="routeTemplate">Route template</param>
        public static void RegisterConfigurationController(
            this HttpConfiguration config,
            string routeTemplate = "api/config")
        {
            config.Routes.MapHttpRoute("Config", routeTemplate, new
            {
                controller = "Configuration",
                action = "Retrieve"
            });
        }
    }
}
