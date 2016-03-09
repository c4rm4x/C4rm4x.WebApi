#region Using

using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts
{
    /// <summary>
    /// Check overall health of your service request
    /// </summary>
    [DataContract]
    public class CheckOverallHealthRequest : CheckHealthRequest
    {
    }
}
