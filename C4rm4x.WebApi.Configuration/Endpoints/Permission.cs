#region Using

using C4rm4x.Tools.Utilities;
using System.Security.Claims;

#endregion

namespace C4rm4x.WebApi.Configuration
{
    /// <summary>
    /// Base class for permission
    /// </summary>
    public abstract class Permission
    {
        /// <summary>
        /// Checks whether the given principal is authorized
        /// </summary>
        /// <param name="principal">The principal</param>
        /// <returns>True if the principal is/owns this permission; false, otherwise</returns>
        public abstract bool IsAuthorized(ClaimsPrincipal principal);
    }

    /// <summary>
    /// Role based permission
    /// </summary>
    public class RoleBasedPermission : Permission
    {
        /// <summary>
        /// Gets the role
        /// </summary>
        public string Rolename { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        protected RoleBasedPermission()
        {

        }

        /// <summary>
        /// Checks whether the given principal is authorized based on role
        /// </summary>
        /// <param name="principal">The principal</param>
        /// <returns>True if the principal has this role; false, otherwise</returns>
        public override bool IsAuthorized(ClaimsPrincipal principal)
        {
            principal.NotNull(nameof(principal));

            return principal.IsInRole(Rolename);
        }
    }

    /// <summary>
    /// Claim based permission
    /// </summary>
    public class ClaimBasedPermission : Permission
    {
        /// <summary>
        /// The claim type
        /// </summary>
        public string ClaimType { get; private set; }

        /// <summary>
        /// The claim value
        /// </summary>
        public string ClaimValue { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        protected ClaimBasedPermission()
        {

        }

        /// <summary>
        /// Checks whether the given principal is authorized based on claim
        /// </summary>
        /// <param name="principal">The principal</param>
        /// <returns>True if the principal owns this claim; false, otherwise</returns>
        public override bool IsAuthorized(ClaimsPrincipal principal)
        {
            principal.NotNull(nameof(principal));

            return principal.HasClaim(ClaimType, ClaimValue);
        }
    }
}
