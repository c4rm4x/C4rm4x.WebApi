#region Using

using C4rm4x.Tools.Security.Jwt;
using C4rm4x.Tools.Utilities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Net.Http.Headers;

#endregion

namespace C4rm4x.WebApi.Security.Jwt
{
    /// <summary>
    /// Delegating handler responsible to check whether or not the JWT is present and valid
    /// for the current HTTP request
    /// </summary>
    public class JwtBasedSecurityMessageHandler 
        : SecurityMessageHandler
    {
        private Func<JwtSecurityTokenHandler> _securityTokenHandlerFactory =
            () => new JwtSecurityTokenHandler();

        private Action<HttpRequestMessage, IPrincipal> _assignPrincipalFactory =
            (request, principal) => request.GetRequestContext().Principal = principal;

        /// <summary>
        /// Gets whether or not the token must be present for the request be processed
        /// </summary>
        public bool ForceAuthentication { get; private set; }

        /// <summary>
        /// Gets the options used to validate the token (when it is present)
        /// </summary>
        public JwtValidationOptions Options { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">Options to validate then token when presents</param>
        /// <param name="forceAuthentication">Indicates whether or not the token must be present to process the request</param>
        public JwtBasedSecurityMessageHandler(
            JwtValidationOptions options,
            bool forceAuthentication = false)
        {
            options.NotNull(nameof(options));

            Options = options;
            ForceAuthentication = forceAuthentication;
        }

        /// <summary>
        /// Returns whether or not the current HTTP rquest is allowed to proceeed
        /// </summary>
        /// <param name="request">The current HTTP request</param>
        /// <returns>True if the current HTTP request is allowed; false, otherwise</returns>
        protected override bool IsRequestAllowed(HttpRequestMessage request)
        {
            request.NotNull(nameof(request));

            string securityToken;
            if (!TryRetrieveToken(request, out securityToken))
                return !ForceAuthentication;

            return ValidateToken(request, securityToken);
        }

        private bool TryRetrieveToken(
            HttpRequestMessage request, 
            out string securityToken)
        {
            securityToken = ExtractCredential(request.Headers.Authorization);

            return !securityToken.IsNullOrEmpty();
        }

        private string ExtractCredential(AuthenticationHeaderValue authorizationHeaderValue)
        {
            if (authorizationHeaderValue.IsNull())
                return string.Empty;

            return authorizationHeaderValue.GetBearerToken();
        }

        private bool ValidateToken(HttpRequestMessage request, string securityToken)
        {
            var handler = _securityTokenHandlerFactory();

            try
            {
                IPrincipal principal;
                var result = handler.TryValidateToken(securityToken, Options, out principal);

                if (result) // Do nothing if validation fails
                {
                    Thread.CurrentPrincipal = principal;
                    _assignPrincipalFactory(request, principal);
                }

                return result;
            }
            catch (Exception)  // Swallow all the exceptions
            {
                return false;
            }
        }        

        /// <summary>
        /// Gets the actual HttpStatusCode. 
        /// In this case, Unauthorized.
        /// </summary>
        protected override HttpStatusCode ForbiddenErrorCode
        {
            get { return HttpStatusCode.Unauthorized; }
        }

        /// <summary>
        /// Sets the security token handler factory
        /// </summary>
        /// <param name="securityTokenHandlerFactory">The factory</param>
        /// <remarks>USE THIS ONLY FOR UNIT TESTING</remarks>
        internal void SetSecurityTokenHandlerFactory(Func<JwtSecurityTokenHandler> securityTokenHandlerFactory)
        {
            securityTokenHandlerFactory.NotNull(nameof(securityTokenHandlerFactory));

            _securityTokenHandlerFactory = securityTokenHandlerFactory;
        }

        /// <summary>
        /// Sets the assign principal factory
        /// </summary>
        /// <param name="assignPrincipalFactory">The factory</param>
        /// <remarks>USE THIS ONLY FOR UNIT TESTING</remarks>
        internal void SetAssignPrincipalFactory(Action<HttpRequestMessage, IPrincipal> assignPrincipalFactory)
        {
            assignPrincipalFactory.NotNull(nameof(assignPrincipalFactory));

            _assignPrincipalFactory = assignPrincipalFactory;
        }
    }
}
