#region Using

using C4rm4x.Tools.Utilities;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Monitoring.Core.Controllers
{
    /// <summary>
    /// Base monitor response result by component
    /// </summary>
    [DataContract]
    public abstract class MonitorResultDto
    {
        /// <summary>
        /// Parameterless constructor for serialization/deserialization
        /// </summary>
        public MonitorResultDto()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="component">The component</param>
        public MonitorResultDto(ComponentDto component)
        {
            component.NotNull(nameof(component));

            Component = component;
        }

        /// <summary>
        /// Gets or sets the component under monitorization
        /// </summary>
        [DataMember (IsRequired = true)]
        public ComponentDto Component { get; set; }
    }
}
