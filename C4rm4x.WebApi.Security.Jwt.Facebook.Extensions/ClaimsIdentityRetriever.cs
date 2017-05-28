#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Security.Jwt.Controllers;
using System.Security.Claims;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Facebook
{
    /// <summary>
    /// Service responsible to retrieve an instance of ClaimIdentity to generate a JWT
    /// validating, first, the user is an authenticated on Facebook and using
    /// its id property to map our own custom implementation
    /// </summary>
    public abstract class ClaimsIdentityRetriever 
        : IClaimsIdentityRetriever
    {
        /// <summary>
        /// Retrieves an instance of ClaimsIdentity based on given credentials
        /// </summary>
        /// <param name="userIdentifier">User identififier (user id on Facebook)</param>
        /// <param name="secret">The serent (temporary token returned by Facebook API to access user information)</param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> RetrieveAsync(
            string userIdentifier, 
            string secret = null)
        {
            var user = await ValidateUserAsync(userIdentifier, secret);

            if (user.IsNull())
                return null;

            return await RetrieveAsync(user);
        }

        private async Task<FBUser> ValidateUserAsync(
            string userId, 
            string token)
        {
            return await FacebookMarketingUserInfoClient.GetUserAsync(userId, token);
        }

        /// <summary>
        /// Retrieves an instance of ClaimsIdentity based on user Facebook id
        /// </summary>
        /// <param name="user">The latest information about the user that FB provides</param>
        /// <returns></returns>
        protected abstract Task<ClaimsIdentity> RetrieveAsync(FBUser user);
    }
}
