#region Using

using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Configuration.Controllers
{
    /// <summary>
    /// Get configuration response
    /// </summary>
    [DataContract]
    public class GetConfigurationResponse
    {
        /// <summary>
        /// Collection of endpoints this user has access to
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<EndpointDto> Endpoints { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GetConfigurationResponse()
        {
            Endpoints = new EndpointDto[] { };
        }
    }
}
