#region Using

using C4rm4x.WebApi.Framework;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Controllers
{
    /// <summary>
    /// Exception to be thrown when claims identity cannot be found
    /// </summary>
    public class UserCredentialsException : ApiException
    {
        private new const string Code = "AUTH_001";
        private new const string Message = "Unrecognized userIdentifier or secret.";

        /// <summary>
        /// Constructor
        /// </summary>
        public UserCredentialsException() : 
            base(Code, Message)
        {
        }
    }
}
