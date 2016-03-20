#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Monitoring.Core.Controllers
{
    /// <summary>
    /// Base monitor response with the result of monitoring all (or the specified subset of)
    /// the components in your system
    /// </summary>
    /// <typeparam name="T">Type of the component monitor response</typeparam>
    [DataContract]
    public class MonitorResponse<T> : ApiResponse
        where T : MonitorResultDto
    {
        /// <summary>
        /// Parameterless constructor for serialization/deserialization
        /// </summary>
        public MonitorResponse()
            : this(new T[] { })
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="results">Collection of all monitor results by component included in the response</param>
        public MonitorResponse(IEnumerable<T> results)
        {
            results.NotNull(nameof(results));

            Results = results;
        }

        /// <summary>
        /// The collection of all monitor results by component
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<T> Results { get; set; }
    }
}
