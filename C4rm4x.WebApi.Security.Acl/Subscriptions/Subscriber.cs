﻿#region Using

using C4rm4x.Tools.Security.Acl;
using C4rm4x.Tools.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

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
        protected Subscriber()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="identifier">The identifier</param>
        /// <param name="secret">The secret</param>
        public Subscriber(
            string identifier,
            string secret)
        {
            Identifier = identifier;
            Secret = secret;
        }

        /// <summary>
        /// What is the identifier of all their requests
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// What is the shared secret between them and this app
        /// </summary>
        public string Secret { get; private set; }        

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

        /// <summary>
        /// Retrieves all the claims for the given subscriber
        /// </summary>
        /// <param name="credentials">The credentias</param>
        /// <returns>The claims for the given subscriber</returns>
        protected virtual IEnumerable<Claim> GetClaims(
            AclClientCredentials credentials)
        {
            yield return new Claim(ClaimTypes.Name, credentials.Identifier);
        }
    }
}
