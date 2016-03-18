﻿#region Using

using C4rm4x.Tools.TestUtilities.Builders;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Test.Controllers
{
    public class CheckHealthResponseBuilder : AbstractBuilder<CheckHealthResponse>
    {
        public CheckHealthResponseBuilder()
        {
            _entity.ComponentStatuses = new ComponentStatusDto[] { };
        }

        public CheckHealthResponseBuilder WithComponnentStatuses(
            params ComponentStatusDto[] componentStatuses)
        {
            _entity.ComponentStatuses = componentStatuses;

            return this;
        }

        public CheckHealthResponseBuilder WithoutComponentStatuses()
        {
            return WithComponnentStatuses();
        }
    }
}
