namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Services
{
    /// <summary>
    /// Interface of a service status retriever responsible to check the health status 
    /// of an specific component
    /// </summary>
    public interface IServiceStatusRetriever
    {
        /// <summary>
        /// The component's identifier which this service is responsible for
        /// </summary>
        object ComponentIdentifier { get; }

        /// <summary>
        /// The component's name with this service is responsible for
        /// </summary>
        string ComponentName { get; }

        /// <summary>
        /// Is the component working as expected?
        /// </summary>
        /// <returns></returns>
        bool IsComponentWorking();
    }
}
