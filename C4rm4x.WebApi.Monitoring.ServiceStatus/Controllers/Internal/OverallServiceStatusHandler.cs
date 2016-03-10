#region Using

using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos;
using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Internal
{
    internal class OverallServiceStatusHandler : 
        AbstractServiceStatusHandler
    {
        private OverallServiceStatusHandler()
            : base()
        { }

        public static IServiceStatusRequestHandler GetInstance()
        {
            return new OverallServiceStatusHandler();
        }

        protected override IEnumerable<ComponentStatusDto> GetComponentStatuses(
            CheckHealthRequest request)
        {
            foreach (var serviceStatusRetriever in ServiceStatusRetrievers)
                yield return GetComponentStatuses(serviceStatusRetriever);
        }

        private ComponentStatusDto GetComponentStatuses(
            IServiceStatusRetriever serviceStatusRetriever)
        {
            return new ComponentStatusDto(
                GetComponent(serviceStatusRetriever),
                serviceStatusRetriever.GetComponentHealthStatus());
        }

        private ComponentDto GetComponent(
            IServiceStatusRetriever serviceStatusRetriever)
        {
            return new ComponentDto(
                serviceStatusRetriever.ComponentIdentifier,
                serviceStatusRetriever.ComponentName);
        }
    }
}
