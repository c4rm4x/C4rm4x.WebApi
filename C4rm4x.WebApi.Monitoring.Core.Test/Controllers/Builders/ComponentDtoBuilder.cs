#region Using

using C4rm4x.Tools.TestUtilities.Builders;
using C4rm4x.WebApi.Monitoring.Core.Controllers;

#endregion

namespace C4rm4x.WebApi.Monitoring.Core.Test.Controllers
{
    public class ComponentDtoBuilder : AbstractBuilder<ComponentDto>
    {
        public ComponentDtoBuilder WithName(string name)
        {
            _entity.Name = name;

            return this;
        }

        public ComponentDtoBuilder WithoutName()
        {
            return WithName(null);
        }

        public ComponentDtoBuilder WithIdentifier(object indentifier)
        {
            _entity.Identifier = indentifier;

            return this;
        }

        public ComponentDtoBuilder WithoutIdentifier()
        {
            return WithIdentifier(null);
        }
    }
}
