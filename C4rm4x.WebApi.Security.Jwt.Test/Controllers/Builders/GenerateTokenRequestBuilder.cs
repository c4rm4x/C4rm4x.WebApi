#region Using

using C4rm4x.Tools.TestUtilities.Builders;
using C4rm4x.WebApi.Security.Jwt.Controllers;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Test.Controllers
{
    internal class GenerateTokenRequestBuilder : 
        AbstractBuilder<GenerateTokenRequest>
    {
        public GenerateTokenRequestBuilder WithoutUserIdentifier()
        {
            return WithUserIdentifier(null);            
        }
        
        public GenerateTokenRequestBuilder WithUserIdentifier(string userIdentifier)
        {
            _entity.UserIdentifier = userIdentifier;

            return this;
        }
    }
}
