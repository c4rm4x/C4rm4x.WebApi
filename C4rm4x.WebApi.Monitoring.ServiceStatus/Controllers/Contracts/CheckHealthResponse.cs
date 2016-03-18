#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers
{
    /// <summary>
    /// Service status response with the the health of all the components
    /// </summary>
    [DataContract]
    public class CheckHealthResponse : ApiResponse
    {
        /// <summary>
        /// Parameterless constructor for serialization/deserialization
        /// </summary>
        public CheckHealthResponse()
            : this(new ComponentStatusDto[] { })
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="componentStatuses">Collection of all component health statuses included in the response</param>
        public CheckHealthResponse(IEnumerable<ComponentStatusDto> componentStatuses)
        {
            componentStatuses.NotNull(nameof(componentStatuses));

            ComponentStatuses = componentStatuses;
        }

        /// <summary>
        /// The collection of all component health statuses
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<ComponentStatusDto> ComponentStatuses { get; set; }

        /// <summary>
        /// Gets the overall health status of your system
        /// </summary>
        [DataMember(IsRequired = true)]
        public SystemHealthStatus OverallHealthStatus
        {
            set { /* Do nothing... but serialization/deserialization requires this */ }
            get
            {
                if (ComponentStatuses.All(c => c.HealthStatus == ComponentHealthStatus.Working))
                    return SystemHealthStatus.Healthy;

                if (ComponentStatuses.All(c => c.HealthStatus == ComponentHealthStatus.Unresponsive))
                    return SystemHealthStatus.Disaster;

                return SystemHealthStatus.WithIssues;
            }
        }
    }

    /// <summary>
    /// Health status of your system
    /// </summary>
    [DataContract]
    public enum SystemHealthStatus
    {
        /// <summary>
        /// All good, your system works well
        /// </summary>
        [EnumMember]
        Healthy,

        /// <summary>
        /// Check your components, it seems some of them are not working
        /// </summary>
        [EnumMember]
        WithIssues,

        /// <summary>
        /// Danger! Your system is on disaster mode
        /// </summary>
        [EnumMember]
        Disaster
    }
}
