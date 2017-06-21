#region Using

using C4rm4x.WebApi.Monitoring.Core;
using System;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus
{
    /// <summary>
    /// Basic implementation of IServiceStatusRetriever
    /// </summary>
    public abstract class AbstractServiceStatusRetriever :
        AbstractMonitorService<bool>,
        IServiceStatusRetriever
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="componentIdentifier">The component's identifier</param>
        /// <param name="componentName">The component's name</param>
        public AbstractServiceStatusRetriever(
            object componentIdentifier,
            string componentName)
            : base(componentIdentifier, componentName)
        { }

        /// <summary>
        /// Is component working as expected?
        /// </summary>
        /// <returns>True if component is working as expected; false, otherwise</returns>
        public override async Task<bool> MonitorAsync()
        {
            var result = true;
                     
            try
            {
                await CheckComponentResponsivenessAsync();
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Checks whether or not the component is responding as expected
        /// </summary>
        /// <remarks>DO THROW an exception when the component is not working as expected</remarks>
        protected abstract Task CheckComponentResponsivenessAsync();
    }
}