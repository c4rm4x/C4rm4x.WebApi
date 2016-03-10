#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Internal
{
    internal class ByComponentsServiceStatusHandler : 
        AbstractServiceStatusHandler
    {
        private ByComponentsServiceStatusHandler()
            : base()
        { }

        public static IServiceStatusRequestHandler GetInstance()
        {
            return new ByComponentsServiceStatusHandler();
        }

        protected override IEnumerable<ComponentStatusDto> GetComponentStatuses(
            CheckHealthRequest request)
        {
            foreach (var component in request.Components)
                yield return GetComponentStatus(component);
        }

        private ComponentStatusDto GetComponentStatus(
            ComponentDto component)
        {
            var serviceStatusRetriever = GetServiceStatusRetriever(component);

            if (serviceStatusRetriever.IsNotNull())
                return GetComponentStatus(component, serviceStatusRetriever);

            return new ComponentStatusDto(component);
        }

        private IServiceStatusRetriever GetServiceStatusRetriever(
            ComponentDto component)
        {
            return ServiceStatusRetrievers
                .FirstOrDefault(r => r.ComponentIdentifier.Equals(component.Identifier));
        }

        private ComponentStatusDto GetComponentStatus(
            ComponentDto component,
            IServiceStatusRetriever serviceStatusRetriever)
        {
            return new ComponentStatusDto(
                component, 
                serviceStatusRetriever.GetComponentHealthStatus());
        }
    }
}
