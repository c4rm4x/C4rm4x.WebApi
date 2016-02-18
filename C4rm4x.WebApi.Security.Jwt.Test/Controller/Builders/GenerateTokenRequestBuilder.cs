#region Using

using C4rm4x.Tools.TestUtilities.Builders;
using C4rm4x.WebApi.Security.Jwt.Controller;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Test.Controller
{
    internal class GenerateTokenRequestBuilder : 
        AbstractBuilder<GenerateTokenRequest>
    {
        public GenerateTokenRequestBuilder WithUserIdentifier(string userIdentifier)
        {
            _entity.UserIdentifier = userIdentifier;

            return this;
        }
    }
}
