using C4rm4x.Tools.HttpUtilities;
using C4rm4x.Tools.Security.Acl;
using C4rm4x.Tools.Utilities;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace C4rm4x.WebApi.Security.Acl
{
    /// <summary>
    /// An filter that verifies the request body has not been compromised.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class DigitalSignedAttribute : ActionFilterAttribute
    {
        private Func<byte[], string, string> _signer = (body, secret) => new AclClientRequestSigner().Sign(body, secret);

        /// <summary>
        /// Gets or sets header name where signature must be found
        /// </summary>
        public string Header { get; set; } = "X-BodyDigitalSignature";

        /// <summary>
        /// Gets the shared secret claim
        /// </summary>
        protected string ClaimType { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="claimType">The shared secret claim</param>
        protected DigitalSignedAttribute(string claimType)
        {
            claimType.NotNullOrEmpty(nameof(claimType));

            ClaimType = claimType;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="claimType">The shared secret claim</param>
        /// <param name="header">The header name where signature must be found</param>
        protected DigitalSignedAttribute(string claimType, string header) 
            : this(claimType)
        {
            header.NotNullOrEmpty(nameof(header));

            Header = header;
        }

        /// <summary>
        /// Called before an action gets executed
        /// Bad request might be returned whether the header is not present, or
        /// its value does not match the expected value based on the given payload        
        /// </summary>
        /// <param name="actionContext">The context</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.NotNull(nameof(actionContext));

            string signature;

            if (!TryRetrieveSignature(actionContext, out signature) || 
                !IsValidSignature(actionContext, signature))
            {
                HandleBadRequest(actionContext);
            }
            else
            {
                base.OnActionExecuting(actionContext);
            }
        }

        private bool TryRetrieveSignature(HttpActionContext actionContext, out string signature)
        {
            signature = actionContext.Request.Headers.GetDigitalSignatureHeader(Header);

            return !signature.IsNullOrEmpty();
        }

        private bool IsValidSignature(HttpActionContext actionContext, string signature)
        {
            var sharedSecret = actionContext.ControllerContext.RequestContext.Principal.GetSharedSecret(ClaimType);

            if (sharedSecret.IsNullOrEmpty()) return false;

            return SignRequest(sharedSecret).Equals(signature);
        }

        private string SignRequest(string sharedSecret)
        {
            return _signer(HttpContextFactory.Current.GetBodyAsByteArray(), sharedSecret);
        }

        /// <summary>
        /// Processes requests that fail validation. 
        /// This default implementation creates a new response with the BadRequest status code. 
        /// </summary>
        /// <param name="actionContext">The context</param>
        protected virtual void HandleBadRequest(HttpActionContext actionContext)
        {
            actionContext.NotNull(nameof(actionContext));

            actionContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                ReasonPhrase = "Missing or invalid {0} header".AsFormat(Header)
            };
        }

        /// <summary>
        /// Sets a new signer to sign the request body using the shared secret
        /// </summary>
        /// <remarks>
        /// USE ONLY FOR UNIT TESTING PURPOSES
        /// </remarks>
        /// <param name="signer">The signer implementation</param>
        internal void SetSigner(Func<byte[], string, string> signer)
        {
            signer.NotNull(nameof(signer));

            _signer = signer;
        }
    }
}
