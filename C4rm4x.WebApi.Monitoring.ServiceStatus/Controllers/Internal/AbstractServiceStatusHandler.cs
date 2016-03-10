#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Internal
{
    #region Interface

    internal interface IServiceStatusRequestHandler
    {
        IHttpActionResult Handle(CheckHealthRequest request);

        void SetServiceStatusRetrievers(
            IEnumerable<IServiceStatusRetriever> serviceStatusRetrievers);
    }

    #endregion

    internal abstract class AbstractServiceStatusHandler : 
        IServiceStatusRequestHandler
    {
        protected IEnumerable<IServiceStatusRetriever> 
            ServiceStatusRetrievers { get; private set; }

        public AbstractServiceStatusHandler()
        { }

        public void SetServiceStatusRetrievers(
            IEnumerable<IServiceStatusRetriever> serviceStatusRetrievers)
        {
            serviceStatusRetrievers.NotNullOrEmpty(nameof(serviceStatusRetrievers));

            ServiceStatusRetrievers = serviceStatusRetrievers;
        }

        public IHttpActionResult Handle(CheckHealthRequest request)
        {
            return Ok(GetCheckHealthResponse(request));
        }

        private CheckHealthResponse GetCheckHealthResponse(
            CheckHealthRequest request)
        {
            return new CheckHealthResponse(
                GetComponentStatuses(request).ToList());
        }

        protected abstract IEnumerable<ComponentStatusDto> GetComponentStatuses(
            CheckHealthRequest request);

        private static IHttpActionResult Ok(CheckHealthResponse response)
        {
            return new OkResult<CheckHealthResponse>(response);
        }
    }
}
