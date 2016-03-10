#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Internal
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
