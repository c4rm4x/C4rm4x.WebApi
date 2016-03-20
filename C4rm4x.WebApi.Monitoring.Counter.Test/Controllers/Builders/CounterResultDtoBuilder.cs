#region Using

using C4rm4x.Tools.TestUtilities.Builders;
using C4rm4x.WebApi.Monitoring.Core.Test.Controllers;
using C4rm4x.WebApi.Monitoring.Counter.Controllers;

#endregion

namespace C4rm4x.WebApi.Monitoring.Counter.Test.Controllers
{
    public class CounterResultDtoBuilder :
        AbstractBuilder<CounterResultDto>
    {
        public CounterResultDtoBuilder()
        {
            _entity.Component = new ComponentDtoBuilder().Build();
        }

        public CounterResultDtoBuilder WithTotal(long total)
        {
            _entity.Total = total;

            return this;
        }
    }
}
