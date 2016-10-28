#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Facebook
{
    internal static class FacebookMarketingUserInfoClient
    {
        public static FBUser GetUser(
            string userId,
            string token)
        {
            var client = new FacebookMarketingClient(token);

            var jsonResult = client.Retrieve("me", "id", "first_name", "picture");

            if (jsonResult == null || 
                !jsonResult.id.Equals(userId, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return Transform(jsonResult);
        }

        private static FBUser Transform(dynamic data)
        {
            return new FBUser(
                data.id,
                data.first_name,
                data.picture?.data?.url ?? string.Empty);
        }
    }
}
