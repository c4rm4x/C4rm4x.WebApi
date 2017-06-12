#region Using

using C4rm4x.Tools.TestUtilities.Builders;
using C4rm4x.WebApi.Configuration.Controllers;

#endregion

namespace C4rm4x.WebApi.Configuration.Test.Controllers.Builders
{
    public class GetConfigurationRequestBuilder :
        AbstractBuilder<GetConfigurationRequest>
    {
        public GetConfigurationRequestBuilder WithAppIdentifier(string appIdentifier)
        {
            _entity.AppIdentifier = appIdentifier;

            return this;
        }

        public GetConfigurationRequestBuilder WithoutAppIdentifier()
        {
            return WithAppIdentifier(null);
        }

        public GetConfigurationRequestBuilder WithVersion(string version)
        {
            _entity.Version = version;

            return this;
        }

        public GetConfigurationRequestBuilder WithoutVersion()
        {
            return WithVersion(null);
        }
    }
}
