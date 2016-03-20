#region Using

using C4rm4x.Tools.TestUtilities.Builders;
using C4rm4x.WebApi.Monitoring.Core.Controllers;
using C4rm4x.WebApi.Monitoring.Counter.Controllers;

#endregion

namespace C4rm4x.WebApi.Monitoring.Counter.Test.Controllers
{
    public class MonitorResponseBuilder : 
        AbstractBuilder<MonitorResponse<CounterResultDto>>
    {
        public MonitorResponseBuilder()
        {
            _entity.Results = new CounterResultDto[] { };
        }
    }
}
