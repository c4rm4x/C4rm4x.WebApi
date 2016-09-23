#region Using

using C4rm4x.Tools.Utilities;
using Facebook;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Facebook
{
    internal class FacebookMarketingClient
    {
        private const string ApiVersion = "v2.7";

        private FacebookClient _client;

        public FacebookMarketingClient(string token)
        {
            _client = new FacebookClient(token);
        }

        public dynamic Retrieve(
            string me,
            params string[] fields)
        {
            try
            {
                return _client.Get(BuildQueryUrl(me, fields));
            }
            catch // If anything happens.... return null
            {
                return null;
            }
        }

        private string BuildQueryUrl(
            string me, 
            params string[] fields)
        {
            return "{0}/{1}?fields={2}".AsFormat(ApiVersion, me, string.Join(",", fields));
        }
    }
}
