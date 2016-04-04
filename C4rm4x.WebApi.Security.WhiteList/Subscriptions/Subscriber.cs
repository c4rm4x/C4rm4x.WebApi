#region Using

using System.Security.Cryptography;
using System.Text;

#endregion

namespace C4rm4x.WebApi.Security.WhiteList.Subscriptions
{
    /// <summary>
    /// Entity that defines a subscriber
    /// </summary>
    public class Subscriber
    {
        /// <summary>
        /// What is the identifier of all their requests
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// What is the shared secret between them and this app
        /// </summary>
        public string Secret { get; private set; }

        internal bool ValidateSecret(string sharedSecret)
        {
            return ComputeHash(sharedSecret) == Secret;
        }

        private static string ComputeHash(string sharedSecret)
        {
            var hash = new MD5CryptoServiceProvider()
                .ComputeHash(Encoding.UTF8.GetBytes(sharedSecret));

            return Encoding.UTF8.GetString(hash);
        }
    }
}
