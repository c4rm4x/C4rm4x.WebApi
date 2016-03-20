#region Using

using C4rm4x.WebApi.Monitoring.Core.Controllers;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Monitoring.Counter.Controllers
{
    /// <summary>
    /// Component with its counter
    /// </summary>
    [DataContract]
    public class CounterResultDto : MonitorResultDto
    {
        /// <summary>
        /// Parameterless constructor for serialization/deserialization
        /// </summary>
        public CounterResultDto()
            : base()
        {
            Total = long.MinValue;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="component">The component</param>
        /// <param name="total">The counter</param>
        public CounterResultDto(
            ComponentDto component,
            long total = -1)
            : base(component)
        {
            Total = total;
        }

        /// <summary>
        /// The counter
        /// </summary>
        [DataMember(IsRequired = true)]
        public long Total { get; set; }
    }
}
