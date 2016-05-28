#region Using

using System;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using C4rm4x.Tools.Security.Acl;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using C4rm4x.Tools.Utilities;

#endregion

namespace C4rm4x.WebApi.Security.Acl.Subscriptions
{
    /// <summary>
    /// Entity that defines a subscriber
    /// </summary>
    public class Subscriber
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal Subscriber()
        { }

        /// <summary>
        /// What is the identifier of all their requests
        /// </summary>
        public string Identifier { get; internal set; }

        /// <summary>
        /// What is the shared secret between them and this app
        /// </summary>
        public string Secret { get; internal set; }        

        internal bool ValidateCredentials(
            AclClientCredentials credentials, 
            out IPrincipal principal)
        {
            principal = null;

            if (ValidateSecret(credentials.Secret))
                principal = GetPrincipal(credentials);

            return principal.IsNotNull();
        }

        private bool ValidateSecret(string sharedSecret)
        {
            return ComputeHash(sharedSecret)
                .Equals(Secret, StringComparison.InvariantCultureIgnoreCase);
        }

        private static string ComputeHash(string sharedSecret)
        {
            var hash = new MD5CryptoServiceProvider()
                .ComputeHash(Encoding.UTF8.GetBytes(sharedSecret));

            return FromHex(hash);
        }

        private static string FromHex(byte[] hash)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));

            return sb.ToString();
        }

        private IPrincipal GetPrincipal(
            AclClientCredentials credentials)
        {
            return new ClaimsPrincipal(
                new ClaimsIdentity(
                    GetClaims(credentials).ToList(),
                    AuthenticationTypes.Basic));
        }

        private IEnumerable<Claim> GetClaims(
            AclClientCredentials credentials)
        {
            yield return new Claim(ClaimTypes.Name, credentials.Identifier);
        }
    }
}
