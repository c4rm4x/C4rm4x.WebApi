#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

#endregion

namespace C4rm4x.WebApi.Security
{
    /// <summary>
    /// An authorization filter that verifies the request's Principal.
    /// </summary>
    /// <remarks>
    /// You can declare multiple of these attributes per action. 
    /// You can also use AllowAnonymousAttribute to disable authorization for a specific action
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class SecuredAttribute : AuthorizationFilterAttribute
    {
        /// <summary>
        /// Gets or sets the authorized role
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the authorized claim
        /// </summary>
        public Claim Claim { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SecuredAttribute()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="role">The role</param>
        public SecuredAttribute(string role)
        {
            role.NotNullOrEmpty(nameof(role));

            Role = role;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="claimType">The claim type</param>
        /// <param name="claimValue">The claim value</param>
        public SecuredAttribute(
            string claimType, 
            string claimValue)
            : this(new Claim(claimType, claimValue))
        {
            claimType.NotNullOrEmpty(nameof(claimType));
            claimValue.NotNullOrEmpty(nameof(claimValue));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="claim">The claim</param>
        public SecuredAttribute(Claim claim)
        {
            claim.NotNull(nameof(claim));

            Claim = claim;
        }

        /// <summary>
        /// Called when an action is being authorized. 
        /// Authorization is denied if
        /// - the request is not associated with any user
        /// - the user is not authenticated,
        /// - the user is authenticated but it is not in the authorized role (if defined)
        ///   or the user does not have the authorized claim (if defined)
        /// </summary>
        /// <param name="actionContext">The context</param>
        /// <param name="cancellationToken">The cancellation token</param>
        public override async Task OnAuthorizationAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            actionContext.NotNull(nameof(actionContext));

            if (SkipAuthorization(actionContext))
                return;

            if (!IsAuthenticated(actionContext))
                await HandleUnauthenticatedRequestAsync(actionContext);
            else if (!IsAuthorized(actionContext))
                await HandleUnauthorizedRequestAsync(actionContext);
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext
                .ActionDescriptor
                .GetCustomAttributes<AllowAnonymousAttribute>()
                .Any();
        }

        /// <summary>
        /// Determines whether access for this particular request is authenticated. 
        /// </summary>
        /// <param name="actionContext">The context</param>
        /// <returns>true if access is authorized; otherwise false</returns>
        protected virtual bool IsAuthenticated(HttpActionContext actionContext)
        {
            actionContext.NotNull(nameof(actionContext));

            var user = actionContext.ControllerContext.RequestContext.Principal;

            return user.IsNotNull() &&
                user.Identity.IsNotNull() &&
                user.Identity.IsAuthenticated;
        }

        /// <summary>
        /// Processes requests that fail authentication. 
        /// This default implementation creates a new response with the Unauthorized status code. 
        /// </summary>
        /// <param name="actionContext">The context</param>
        protected virtual async Task HandleUnauthenticatedRequestAsync(HttpActionContext actionContext)
        {
            actionContext.NotNull(nameof(actionContext));

            actionContext.Response = await Task.FromResult(
                new HttpResponseMessage(HttpStatusCode.Unauthorized));
        }

        /// <summary>
        /// Determines whether access for this particular request is authorized. 
        /// Authorization is denied when the user is not in the authorized role (if defined)
        /// or does not have the authorized claim (if defined)
        /// </summary>
        /// <param name="actionContext">The context</param>
        /// <returns>true if access is authorized; otherwise false</returns>
        protected virtual bool IsAuthorized(HttpActionContext actionContext)
        {
            actionContext.NotNull(nameof(actionContext));

            var user = actionContext.ControllerContext.RequestContext.Principal;

            if (NotAuthorizedRole(user) || NotAuthorizedClaim(user as ClaimsPrincipal))
                return false;

            return true;
        }

        private bool NotAuthorizedRole(IPrincipal user)
        {
            return !Role.IsNullOrEmpty() && !user.IsInRole(Role);
        }

        private bool NotAuthorizedClaim(ClaimsPrincipal user)
        {
            return user.IsNotNull() && 
                Claim.IsNotNull() && 
                !user.HasClaim(Claim.Type, Claim.Value);
        }

        /// <summary>
        /// Processes requests that fail authorization. 
        /// This default implementation creates a new response with the Forbidden status code. 
        /// </summary>
        /// <param name="actionContext">The context</param>
        protected virtual async Task HandleUnauthorizedRequestAsync(HttpActionContext actionContext)
        {
            actionContext.NotNull(nameof(actionContext));

            actionContext.Response = await Task.FromResult(
                new HttpResponseMessage(HttpStatusCode.Forbidden));
        }
    }
}