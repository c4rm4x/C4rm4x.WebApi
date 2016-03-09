#region Using

using C4rm4x.Tools.Utilities;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos
{
    /// <summary>
    /// Component with its health status
    /// </summary>
    [DataContract]
    public class ComponentStatusDto
    {
        /// <summary>
        /// Parameterless constructor for serialization/deserialization
        /// </summary>
        public ComponentStatusDto()
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
        {
            component.NotNull(nameof(component));

            Component = component;
            HealthStatus = healthStatus;
        }

        /// <summary>
        /// The component
        /// </summary>
        [DataMember(IsRequired = true)]
        public ComponentDto Component { get; set; }

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
