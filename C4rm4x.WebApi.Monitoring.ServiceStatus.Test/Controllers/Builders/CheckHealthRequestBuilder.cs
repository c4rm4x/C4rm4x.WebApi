#region Using

using C4rm4x.Tools.TestUtilities.Builders;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Test.Controllers.Builders
{
    public class CheckHealthRequestBuilder :
        AbstractBuilder<CheckHealthRequest>
    {
        public CheckHealthRequestBuilder()
        {
            _entity.Components = new List<ComponentDto>();
        }

        public CheckHealthRequestBuilder WithoutComponents()
        {
            return WithComponents();
        }

        public CheckHealthRequestBuilder WithComponents(
            params ComponentDto[] components)
        {
            _entity.Components = components.ToList();

            return this;
        }
    }
}
