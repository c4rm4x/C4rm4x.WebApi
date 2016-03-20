#region Using

using C4rm4x.Tools.TestUtilities.Builders;
using C4rm4x.WebApi.Monitoring.Core.Controllers;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Test.Controllers
{
    public class MonitorResponseBuilder : 
        AbstractBuilder<MonitorResponse<ComponentStatusDto>>
    {
        public MonitorResponseBuilder()
        {
            _entity.Results = new ComponentStatusDto[] { };
        }
    }
}
