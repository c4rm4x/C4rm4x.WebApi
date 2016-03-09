#region Using

using C4rm4x.WebApi.Framework;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts
{
    /// <summary>
    /// Basic request to check health of your service
    /// </summary>
    [DataContract]
    public abstract class CheckHealthRequest : ApiRequest
    {
    }
}
