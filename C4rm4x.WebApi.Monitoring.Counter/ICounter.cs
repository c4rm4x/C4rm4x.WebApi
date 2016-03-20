#region Using

using C4rm4x.WebApi.Monitoring.Core;

#endregion

namespace C4rm4x.WebApi.Monitoring.Counter
{
    /// <summary>
    /// Interface of a counter responsible to count a relevant
    /// piece of information of an specific component
    /// </summary>
    public interface ICounter : IMonitorService<long>
    { }
}
