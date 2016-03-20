#region Using

using C4rm4x.WebApi.Monitoring.Core;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus
{
    /// <summary>
    /// Interface of a service status retriever responsible to check the health status 
    /// of an specific component
    /// </summary>
    public interface IServiceStatusRetriever : IMonitorService<bool>
    { }
}
