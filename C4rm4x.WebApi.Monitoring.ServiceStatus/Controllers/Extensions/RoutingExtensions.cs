#region Using

using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Extensions
{
    /// <summary>
    /// Routing extensions
    /// </summary>
    public static class RoutingExtensions
    {
        /// <summary>
        /// Register service status controller within your application's HttpConfiguration
        /// </summary>
        /// <param name="config">The config</param>
        /// <param name="routeTemplate">Route template</param>
        public static void RegisterServiceStatusController(
            this HttpConfiguration config,
            string routeTemplate = "api/health")
        {
            config
                .Routes
                .MapHttpRoute("Health", routeTemplate,
                new
                {
                    controller = "ServiceStatus",
                    action = "CheckHealth"
                });
        }
    }
}
