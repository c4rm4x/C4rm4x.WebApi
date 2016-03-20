#region Using

using C4rm4x.WebApi.Framework.Log;
using C4rm4x.WebApi.Monitoring.Core.Controllers;
using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers
{
    /// <summary>
    /// Basic implementation of an ApiController responsible for checking 
    /// the health of your system
    /// </summary>
    public class ServiceStatusController : 
        MonitorController<bool, ComponentStatusDto>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="serviceStatusRetrievers">The list of all instances that implement interface IServiceStatusRetriever</param>
        public ServiceStatusController(
            ILog logger,
            IEnumerable<IServiceStatusRetriever> serviceStatusRetrievers)
            : base(logger, serviceStatusRetrievers, Transform)
        { }

        private static ComponentStatusDto Transform(
            ComponentDto component, bool isComponentWorking)
        {
            return new ComponentStatusDto(
                component, 
                GetComponentHealthStatus(isComponentWorking));         
        }

        private static ComponentHealthStatus GetComponentHealthStatus(
            bool isComponentWorking)
        {
            return isComponentWorking
                ? ComponentHealthStatus.Working
                : ComponentHealthStatus.Unresponsive;
        }
    }
}
