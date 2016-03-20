#region Using

using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Monitoring.Counter.Controllers
{
    /// <summary>
    /// Routing extensions
    /// </summary>
    public static class RoutingExtensions
    {
        /// <summary>
        /// Register counter controller within your application's HttpConfiguration
        /// </summary>
        /// <param name="config">The config</param>
        /// <param name="routeTemplate">Route template</param>
        public static void RegisterCounterController(
            this HttpConfiguration config,
            string routeTemplate = "api/counter")
        {
            config
                .Routes
                .MapHttpRoute("Counter", routeTemplate,
                new
                {
                    controller = "Counter",
                    action = "Monitor"
                });
        }
    }
}
