#region Using

using C4rm4x.Tools.TestUtilities.Builders;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Test.Controllers
{
    public class ComponentStatusDtoBuilder : AbstractBuilder<ComponentStatusDto>
    {
        public ComponentStatusDtoBuilder()
        {
            _entity.Component = new ComponentDtoBuilder().Build();
        }

        public ComponentStatusDtoBuilder WithHealthStatus(ComponentHealthStatus healthStatus)
        {
            _entity.HealthStatus = healthStatus;

            return this;
        }
    }
}
