#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos;
using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts
{
    /// <summary>
    /// Check specified components' health of your service request
    /// </summary>
    [DataContract]
    public class CheckComponentsHealthRequest : CheckHealthRequest
    {
        /// <summary>
        /// Parameterless constructor for serialization/deserialization
        /// </summary>
        public CheckComponentsHealthRequest()
            : this(new ComponentDto[] { })
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="components">Collection of components to check their health in your system</param>
        public CheckComponentsHealthRequest(IEnumerable<ComponentDto> components)
        {
            components.NotNull(nameof(components));

            Components = components;
        }

        /// <summary>
        /// Collection of components to check their health in your system
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<ComponentDto> Components { get; set; }
    }
}
