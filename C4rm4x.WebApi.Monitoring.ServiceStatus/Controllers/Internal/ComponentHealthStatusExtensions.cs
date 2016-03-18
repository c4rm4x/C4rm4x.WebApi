#region Using

using C4rm4x.Tools.Utilities;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers
{
    internal static class ComponentHealthStatusExtensions
    {
        public static ComponentHealthStatus GetComponentHealthStatus(
            this IServiceStatusRetriever serviceStatusRetriever)
        {
            serviceStatusRetriever.NotNull(nameof(serviceStatusRetriever));

            return serviceStatusRetriever.IsComponentWorking()
                ? ComponentHealthStatus.Working
                : ComponentHealthStatus.Unresponsive;
        }
    }
}
