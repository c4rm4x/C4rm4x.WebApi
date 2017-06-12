#region Using

using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Configuration
{
    /// <summary>
    /// Endpoint settings
    /// </summary>
    public class Endpoint
    {
        /// <summary>
        /// Gest the endpoint context
        /// </summary>
        public string Context { get; private set; }

        /// <summary>
        /// Gets the endpoint url
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Gets the collection of permissions that the user must have (at least one) to reach this endpoint
        /// </summary>
        public virtual ICollection<Permission> Permissions { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        protected Endpoint()
        {
            Permissions = new HashSet<Permission>();
        }
    }
}
