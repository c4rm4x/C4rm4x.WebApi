#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos;
using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts
{
    /// <summary>
    /// Basic request to check health of your service
    /// </summary>
    /// <remarks>
    /// If list of components is null or empty, it will check the health status of all the components within your system
    /// </remarks>
    [DataContract]
    public class CheckHealthRequest : ApiRequest
    {
        /// <summary>
        /// Parameterless constructor for serialization/deserialization
        /// </summary>
        public CheckHealthRequest()
            : this(new ComponentDto[] { })
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="components">Collection of components to check their health in your system</param>
        public CheckHealthRequest(IEnumerable<ComponentDto> components)
        {
            components.NotNull(nameof(components));

            Components = components;
        }

        /// <summary>
        /// Collection of components to check their health in your system (all when empty)
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<ComponentDto> Components { get; set; }
    }
}
