#region using

using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Configuration
{
    /// <summary>
    /// App configuration per version
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        /// The app identifier
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// The version
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// The collection of all the endpoints related to this app/version
        /// </summary>
        public ICollection<Endpoint> Endpoints { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        protected AppConfiguration()
        {
            Endpoints = new HashSet<Endpoint>();
        }
    }
}
