#region Using

using C4rm4x.Tools.HttpUtilities;
using C4rm4x.Tools.Utilities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;

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
        private const string AuthorizationHeader = "Authorization";

        private Func<JwtSecurityTokenHandler> _securityTokenHandlerFactory =
            () => new JwtSecurityTokenHandler();

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

            return ValidateToken(securityToken);
        }

        private bool TryRetrieveToken(
            HttpRequestMessage request, 
            out string securityToken)
        {
            securityToken = null;

            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues(AuthorizationHeader, out authzHeaders))
                return false;

            return !(securityToken = authzHeaders.First()).IsNullOrEmpty();
        }

        private bool ValidateToken(string securityToken)
        {
            var handler = _securityTokenHandlerFactory();

            try
            {
                IPrincipal principal;
                var result = handler.TryValidateToken(securityToken, Options, out principal);

                Thread.CurrentPrincipal = 
                    HttpContextFactory.Current.User = principal;

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
        public void SetSecurityTokenHandlerFactory(Func<JwtSecurityTokenHandler> securityTokenHandlerFactory)
        {
            securityTokenHandlerFactory.NotNull(nameof(securityTokenHandlerFactory));

            _securityTokenHandlerFactory = securityTokenHandlerFactory;
        }
    }
}
