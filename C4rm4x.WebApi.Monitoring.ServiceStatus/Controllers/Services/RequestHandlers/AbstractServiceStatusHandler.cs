#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Framework.Validation;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos;
using System.Collections.Generic;
using System.Web.Http;
using System;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Services
{
    internal abstract class AbstractServiceStatusHandler : 
        IServiceStatusRequestHandler
    {
        protected IEnumerable<IServiceStatusRetriever> 
            ServiceStatusRetrievers { get; private set; }

        public AbstractServiceStatusHandler(
            IEnumerable<IServiceStatusRetriever> serviceStatusRetrievers)
        {
            serviceStatusRetrievers.NotNullOrEmpty(nameof(serviceStatusRetrievers));

            ServiceStatusRetrievers = serviceStatusRetrievers;
        }

        public IHttpActionResult Handle(CheckHealthRequest request)
        {
            Validate(request);

            return Ok(GetCheckHealthResponse(request));
        }

        private void Validate(CheckHealthRequest request)
        {
            GetValidator()
                .ThrowIf(request);
        }

        protected abstract IValidator GetValidator();

        private CheckHealthResponse GetCheckHealthResponse(
            CheckHealthRequest request)
        {
            return new CheckHealthResponse(
                GetComponentStatuses(request));
        }

        protected abstract IEnumerable<ComponentStatusDto> GetComponentStatuses(
            CheckHealthRequest request);

        private static IHttpActionResult Ok(CheckHealthResponse response)
        {
            return new OkResult<CheckHealthResponse>(response);
        }
    }
}
