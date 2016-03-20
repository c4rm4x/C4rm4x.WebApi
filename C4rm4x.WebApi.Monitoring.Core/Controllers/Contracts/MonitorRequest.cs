#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Monitoring.Core.Controllers
{
    /// <summary>
    /// Basic request to monitor your service
    /// </summary>
    /// <remarks>
    /// If list of components is null or empty, it will monitor all the components within your system
    /// </remarks>
    [DataContract]
    public class MonitorRequest : ApiRequest
    {
        /// <summary>
        /// Parameterless constructor for serialization/deserialization
        /// </summary>
        public MonitorRequest()
            : this(new ComponentDto[] { })
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="components">Collection of components to monitor in your system</param>
        public MonitorRequest(IEnumerable<ComponentDto> components)
        {
            components.NotNull(nameof(components));

            Components = components.ToList();
        }

        /// <summary>
        /// Collection of components to monitor in your system (all when empty)
        /// </summary>
        [DataMember(IsRequired = true)]
        public List<ComponentDto> Components { get; set; }
    }
}
