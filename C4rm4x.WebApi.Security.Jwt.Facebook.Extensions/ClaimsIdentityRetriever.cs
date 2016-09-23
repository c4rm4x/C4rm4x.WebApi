#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Security.Jwt.Controllers;
using System.Security.Claims;

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
        public ClaimsIdentity Retrieve(
            string userIdentifier, 
            string secret = null)
        {
            var user = ValidateUser(userIdentifier, secret);

            return Retrieve(user);
        }

        private FBUser ValidateUser(
            string userId, 
            string token)
        {
            var user = FacebookMarketingUserInfoClient.GetUser(userId, token);

            if (user.IsNull())
                throw new UserCredentialsException();

            return user;
        }

        /// <summary>
        /// Retrieves an instance of ClaimsIdentity based on user Facebook id
        /// </summary>
        /// <param name="user">The latest information about the user that FB provides</param>
        /// <returns></returns>
        protected abstract ClaimsIdentity Retrieve(FBUser user);
    }
}
