#region Using

using C4rm4x.WebApi.Monitoring.Core.Controllers;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers
{
    /// <summary>
    /// Component with its health status
    /// </summary>
    [DataContract]
    public class ComponentStatusDto : MonitorResultDto
    {
        /// <summary>
        /// Parameterless constructor for serialization/deserialization
        /// </summary>
        public ComponentStatusDto()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="component">The component</param>
        /// <param name="healthStatus">Component health status</param>
        public ComponentStatusDto(
            ComponentDto component,
            ComponentHealthStatus healthStatus = ComponentHealthStatus.Unknown)
            : base(component)
        {
            HealthStatus = healthStatus;
        }

        /// <summary>
        /// Component health status
        /// </summary>
        [DataMember(IsRequired = true)]
        public ComponentHealthStatus HealthStatus { get; set; }
    }

    /// <summary>
    /// Component heath status
    /// </summary>
    [DataContract]
    public enum ComponentHealthStatus
    {
        /// <summary>
        /// The specified component has not been found or no service is monitoring this
        /// </summary>
        [EnumMember]
        Unknown,

        /// <summary>
        /// The specified component is working as expected
        /// </summary>
        [EnumMember]
        Working,

        /// <summary>
        /// The specified component does not respond
        /// </summary>
        [EnumMember]
        Unresponsive
    }
}
