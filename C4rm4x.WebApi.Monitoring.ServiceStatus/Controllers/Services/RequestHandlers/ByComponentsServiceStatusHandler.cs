#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Validators;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Services
{
    internal class ByComponentsServiceStatusHandler : 
        AbstractServiceStatusHandler
    {
        private ByComponentsServiceStatusHandler(
            IEnumerable<IServiceStatusRetriever> serviceStatusRetrievers)
            : base(serviceStatusRetrievers)
        { }

        public static IServiceStatusRequestHandler GetInstance(
            IEnumerable<IServiceStatusRetriever> serviceStatusRetrievers)
        {
            return new ByComponentsServiceStatusHandler(serviceStatusRetrievers);
        }

        protected override IValidator GetValidator()
        {
            return new CheckComponentsHealthRequestValidator();
        }

        protected override IEnumerable<ComponentStatusDto> GetComponentStatuses(
            CheckHealthRequest request)
        {
            var actualRequest = request as CheckComponentsHealthRequest;

            foreach (var component in actualRequest.Components)
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
