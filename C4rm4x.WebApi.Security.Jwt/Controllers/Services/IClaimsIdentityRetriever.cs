﻿#region Using

using System.Security.Claims;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Controllers
{
    /// <summary>
    /// Interface to retrieve an instance of ClaimIdentity to generate a JWT
    /// </summary>
    public interface IClaimsIdentityRetriever
    {
        /// <summary>
        /// Retrieves an instance of ClaimsIdentity based on given credentials
        /// </summary>
        /// <param name="userIdentifier">User identifier</param>
        /// <param name="secret">User's secret</param>
        /// <returns></returns>
        Task<ClaimsIdentity> RetrieveAsync(
            string userIdentifier, 
            string secret = null);
    }
}
