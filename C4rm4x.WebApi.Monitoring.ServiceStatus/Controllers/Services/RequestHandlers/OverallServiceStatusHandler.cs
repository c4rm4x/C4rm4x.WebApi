#region Using

using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Validators;
using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Services
{
    internal class OverallServiceStatusHandler : 
        AbstractServiceStatusHandler
    {
        private OverallServiceStatusHandler(
            IEnumerable<IServiceStatusRetriever> serviceStatusRetrievers)
            : base(serviceStatusRetrievers)
        { }

        public static IServiceStatusRequestHandler GetInstance(
            IEnumerable<IServiceStatusRetriever> serviceStatusRetrievers)
        {
            return new OverallServiceStatusHandler(serviceStatusRetrievers);
        }

        protected override IValidator GetValidator()
        {
            return new CheckOverallHealthRequestValidator();
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
