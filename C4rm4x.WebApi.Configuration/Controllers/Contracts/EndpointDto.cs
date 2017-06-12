#region Using

using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Configuration.Controllers
{
    /// <summary>
    /// Endpoint settings
    /// </summary>
    [DataContract]
    public class EndpointDto
    {
        /// <summary>
        /// Endpoint context
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Context { get; set; }

        /// <summary>
        /// Endpoint url
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Url { get; set; }
    }
}
