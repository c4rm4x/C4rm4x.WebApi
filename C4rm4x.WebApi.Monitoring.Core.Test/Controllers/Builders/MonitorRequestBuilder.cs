#region Using

using C4rm4x.Tools.TestUtilities.Builders;
using C4rm4x.WebApi.Monitoring.Core.Controllers;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Monitoring.Core.Test.Controllers
{
    public class MonitorRequestBuilder : 
        AbstractBuilder<MonitorRequest>
    {
        public MonitorRequestBuilder()
        {
            _entity.Components = new List<ComponentDto>();
        }

        public MonitorRequestBuilder WithoutComponents()
        {
            return WithComponents();
        }

        public MonitorRequestBuilder WithComponents(
            params ComponentDto[] components)
        {
            _entity.Components = components.ToList();

            return this;
        }
    }
}
