#region Using

using C4rm4x.WebApi.Framework;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Configuration.Controllers
{
    /// <summary>
    /// Get configuration request
    /// </summary>
    [DataContract]
    public class GetConfigurationRequest : ApiRequest
    {
        /// <summary>
        /// App identifier
        /// </summary>
        [DataMember(IsRequired = true)]
        public string AppIdentifier { get; set; }

        /// <summary>
        /// App version
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Version { get; set; }
    }
}
